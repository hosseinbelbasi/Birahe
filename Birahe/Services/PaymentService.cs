using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;

namespace Birahe.EndPoint.Services;

public class PaymentService {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly PaymentRepository _paymentRepository;
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;

    private string MerchantId => _config["Zarinpal:MerchantId"]!;
    private string CallbackUrl => _config["Zarinpal:CallbackUrl"]!;
    private string ZarinPalUrl => _config["ZarinPal:ZarinPalUrl"]!;
    private string RedirectToZarinPalUrl => _config["ZarinPal:RedirectToZarinPal"]!;
    private string VerifyPaymentUrl => _config["ZarinPal:VerifyPaymentUrl"]!;




    public PaymentService(HttpClient httpClient, IConfiguration config, PaymentRepository paymentRepository, ApplicationContext context, UserRepository userRepository) {
        _httpClient = httpClient;
        _config = config;
        _paymentRepository = paymentRepository;
        _context = context;
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<string>> CreatePaymentAsync(CreatePaymentDto dto) {
        var amount = 1000;
        var description = "";

        var payment = new Payment
        {
            UserId = dto.UserId,
            Amount = amount,
            Status = PaymentStatus.Pending
        };

        await _paymentRepository.AddAsync(payment);
        await _context.SaveChangesAsync();

        var request = new
        {
            merchant_id = MerchantId,
            amount = amount,
            description = description,
            callback_url = $"{CallbackUrl}?authority={{Authority}}"
        };

        var response = await _httpClient.PostAsJsonAsync(ZarinPalUrl, request);
        if (!response.IsSuccessStatusCode)
            return ServiceResult<string>.Fail("Failed to connect to Zarinpal.");

        var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResponse>();

        if (result?.Data?.Code != 100)
            return ServiceResult<string>.Fail($"Zarinpal error: {result?.Errors?.Message ?? "Unknown"}");

        payment.Authority = result.Data.Authority;
        await _context.SaveChangesAsync();

        var redirectUrl = $"{RedirectToZarinPalUrl}/{result.Data.Authority}";
        return ServiceResult<string>.Ok(redirectUrl);
    }

    public async Task<ServiceResult<Payment>> VerifyPaymentAsync(VerifyPaymentDto dto)
    {
        var payment = await _paymentRepository
            .GetByAuthorityAsync(dto.Authority);

        if (payment == null)
            return ServiceResult<Payment>.Fail("Payment not found.");

        if (dto.Status != "OK")
        {
            payment.Status = PaymentStatus.Failed;
            await _context.SaveChangesAsync();
            return ServiceResult<Payment>.Fail("Payment was canceled by user.");
        }

        var request = new
        {
            merchant_id = MerchantId,
            amount = payment.Amount,
            authority = dto.Authority
        };

        var response = await _httpClient.PostAsJsonAsync(VerifyPaymentUrl, request);
        if (!response.IsSuccessStatusCode)
            return ServiceResult<Payment>.Fail("Verification failed to connect.");

        var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResponse>();

        if (result?.Data?.Code == 100)
        {
            payment.Status = PaymentStatus.Success;
            payment.RefId = result.Data.RefId.ToString();
            var activated = await _userRepository.ActivateUser(payment.Authority);
            if (!activated)
            {
                payment.Status = PaymentStatus.Failed;
                await _context.SaveChangesAsync();
                return ServiceResult<Payment>.Fail("Payment verified but user activation failed.");
            }
            await _context.SaveChangesAsync();
            return ServiceResult<Payment>.Ok(payment);
        }

        payment.Status = PaymentStatus.Failed;
        await _context.SaveChangesAsync();
        return ServiceResult<Payment>.Fail($"Verification failed: {result?.Errors?.Message ?? "Unknown"}");
    }
}
