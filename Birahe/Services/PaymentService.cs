using System.Net;
using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Services;

public class PaymentService {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly PaymentRepository _paymentRepository;
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    private string MerchantId => _config["Zarinpal:MerchantId"]!;
    private string CallbackUrl => _config["Zarinpal:CallbackUrl"]!;
    private string ZarinPalUrl => _config["ZarinPal:ZarinPalUrl"]!;
    private string RedirectToZarinPalUrl => _config["ZarinPal:RedirectToZarinPal"]!;
    private string VerifyPaymentUrl => _config["ZarinPal:VerifyPaymentUrl"]!;
    private string PaymentAmount => _config["Payment:Amount"]!;


    public PaymentService(HttpClient httpClient, IConfiguration config, PaymentRepository paymentRepository,
        ApplicationContext context, UserRepository userRepository, IMapper mapper) {
        _httpClient = httpClient;
        _config = config;
        _paymentRepository = paymentRepository;
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task<ServiceResult<string>> CreatePaymentAsync(int id, CreatePaymentDto dto) {
        var user = await _userRepository.FindUser(id);

        if (user == null) {
            return ServiceResult<string>.Fail("کاربر پیدا نشد");
        }

        var userHasPayed = user.Payments.Any(p => p.Status == PaymentStatus.Success);
        if (userHasPayed) {
            return ServiceResult<string>.Fail("کاربر قبلا ثبت نام کرده است");
        }

        var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.Key == dto.Discount);
        var amount = await GetFinalAmount(discount);
        var description = "هزینه ثبت نام در مسابقه بیراهه";

        var payment = new Payment {
            UserId = id,
            Amount = amount,
            Status = PaymentStatus.Pending,
            Authority = ""
        };

        await _paymentRepository.AddAsync(payment);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            await _userRepository.LogicalDelete(id);
            await _context.SaveChangesAsync();
            return ServiceResult<string>.Fail("خظای ناشناخته", ErrorType.ServerError);
        }

        var request = new {
            merchant_id = MerchantId,
            amount = amount,
            description = description,
            callback_url = $"{CallbackUrl}"
        };

        HttpResponseMessage response;
        try {
            response = await _httpClient.PostAsJsonAsync(ZarinPalUrl, request);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            await _userRepository.LogicalDelete(id);
            await _context.SaveChangesAsync();
            return ServiceResult<string>.Fail("خطای درگاه پرداخت!", ErrorType.ServerError);
        }

        if (!response.IsSuccessStatusCode) {
            await _userRepository.LogicalDelete(id);
            await _context.SaveChangesAsync();
            return ServiceResult<string>.Fail("خطا در اتصال یه در گاه پرداخت !", ErrorType.ServerError);
        }

        var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResponse>();

        if (result?.data?.code != 100) {
            await _userRepository.LogicalDelete(id);
            await _context.SaveChangesAsync();
            var errorMessage = "";
            if (result?.errors != null) {
                foreach (var error in result?.errors) {
                    errorMessage += error?.message + "\n";
                }
            }

            return ServiceResult<string>.Fail($"خطای درگاه پرداخت!: {errorMessage}");
        }

        payment.Authority = result.data.authority;
        await _context.SaveChangesAsync();

        var redirectUrl = $"{RedirectToZarinPalUrl}{result.data.authority}";
        return ServiceResult<string>.Ok(redirectUrl);
    }

    public async Task<ServiceResult<PaymentVerifiedDto>> VerifyPaymentAsync(VerifyPaymentDto dto) {
        var payment = await _paymentRepository
            .GetByAuthorityAsync(dto.Authority);


        if (payment == null)
            return ServiceResult<PaymentVerifiedDto>.Fail("کد پرداخت اشتباه است");

        var userId = payment.UserId ?? -1;

        if (userId == -1) {
            return ServiceResult<PaymentVerifiedDto>.Fail("کاربر یافت نشد!");
        }


        // if (payment.UserId != userId) {
        //     return ServiceResult<PaymentVerifiedDto>.Fail("کد پرداخت متعلق یه کاربر درخواست دهنده نیست!");
        // }

        if (payment.Status == PaymentStatus.Success)
            return ServiceResult<PaymentVerifiedDto>.Fail("این پرداخت قبلاً با موفقیت تأیید شده است.");

        if (dto.Status != "OK") {
            payment.Status = PaymentStatus.Failed;
            await _userRepository.LogicalDelete(userId);
            await _context.SaveChangesAsync();
            return ServiceResult<PaymentVerifiedDto>.Fail("پرداخت توسط کاربر لغو شد!");
        }

        var request = new {
            merchant_id = MerchantId,
            amount = payment.Amount,
            authority = dto.Authority
        };

        HttpResponseMessage response;
        try {
            response = await _httpClient.PostAsJsonAsync(VerifyPaymentUrl, request);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return ServiceResult<PaymentVerifiedDto>.Fail("خطای درگاه پرداخت!", ErrorType.ServerError);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized) {
            return ServiceResult<PaymentVerifiedDto>.Fail("پرداخت توسط کاربر لغو شد!");
        }

        if (!response.IsSuccessStatusCode)
            return ServiceResult<PaymentVerifiedDto>.Fail("خطا در اتصال به درگاه پرداخت!");


        var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResponse>();

        /*if (result?.data?.code == 100)
        {
            payment.Status = PaymentStatus.Success;
            payment.ref_id = result.data.ref_id.ToString();
            var activated = await _userRepository.ActivateUser(payment.UserId);
            if (!activated)
            {
                payment.Status = PaymentStatus.Failed;
                await _context.SaveChangesAsync();
                return ServiceResult<PaymentVerifiedDto>.Fail("Payment verified but user activation failed.");
            }
            await _context.SaveChangesAsync();

            var paymentDto = _mapper.Map<PaymentVerifiedDto>(payment);
            return ServiceResult<PaymentVerifiedDto>.Ok(paymentDto);
        }*/
        /*payment.Status = PaymentStatus.Failed;
        await _context.SaveChangesAsync();
        return ServiceResult<PaymentVerifiedDto>.Fail($"Verification failed: {result?.errors }");*/

        if (result?.data?.code == 101) {
            return ServiceResult<PaymentVerifiedDto>.Ok(null, "این پرداخت قبلا تایید شده است");
        }

        if (result?.data?.code != 100) {
            payment.Status = PaymentStatus.Failed;
            payment.ModificationDateTime = DateTime.UtcNow;
            payment.RefId = result?.data?.ref_id?.ToString();
            Console.WriteLine("payment" + payment.RefId);
            Console.WriteLine("result" + payment.RefId);

            await _userRepository.LogicalDelete(userId);
            await _context.SaveChangesAsync();
            var paymentDto = _mapper.Map<PaymentVerifiedDto>(payment);

            return ServiceResult<PaymentVerifiedDto>.Ok(paymentDto, $"تأیید پرداخت ناموفق بود: {result?.errors}");
        }

        payment.Status = PaymentStatus.Success;
        payment.RefId = result.data.ref_id?.ToString();

        var activated = await _userRepository.ActivateUser(userId);
        if (!activated) {
            payment.Status = PaymentStatus.Success;
            await _context.SaveChangesAsync();

            return ServiceResult<PaymentVerifiedDto>.Fail(
                "پرداخت موفق بود اما فعال‌سازی حساب کاربر انجام نشد. لطفا با پشتیبانی تماس بگیرید");
        }

        await _context.SaveChangesAsync();
        payment.RefId = result?.data?.ref_id?.ToString();
        Console.WriteLine("=====================" + payment.RefId + "=====================");
        var paymentDto2 = _mapper.Map<PaymentVerifiedDto>(payment);
        return ServiceResult<PaymentVerifiedDto>.Ok(paymentDto2);
    }

    public async Task<ServiceResult<int>> CalculateAmount(string key) {
        var amount = await GetFinalAmount();
        if (string.IsNullOrEmpty(key)) {
            return ServiceResult<int>.Ok(amount);
        }

        var discount = await _paymentRepository.FindDiscount(key);
        if (discount == null) {
            return ServiceResult<int>.Ok(amount, "این کد تخفیف معتبر نیست!", success: false);
        }

        amount = await GetFinalAmount(discount);
        return ServiceResult<int>.Ok(amount);
    }

    private async Task<int> GetFinalAmount(Discount? discount = null) {
        var earlyAmount = 1800000;
        var normalAmount = 2400000;
        int amount;
        var signupStartConfig = await _context.ContestConfigs.FirstOrDefaultAsync(cc => cc.Key == "Contest");
        var startTime = signupStartConfig.StartTime;

        var flag = DateTime.UtcNow < startTime.AddDays(3);

        if (flag) {
            amount = earlyAmount;
        }
        else {
            amount = normalAmount;
        }

        var finalAmount = amount;
        if (discount != null) {
            finalAmount = (amount) * (100 - discount.Percent) / 100;
        }

        return finalAmount;
    }
}