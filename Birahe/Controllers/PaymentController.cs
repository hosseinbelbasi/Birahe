using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase {
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService) {
        _paymentService = paymentService;
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentDto createPaymentDto) {
        var result = await _paymentService.CreatePaymentAsync(createPaymentDto);
        return this.MapServiceResult(result);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyPaymentDto verifyPaymentDto) {
        var result = await _paymentService.VerifyPaymentAsync(verifyPaymentDto);
        return this.MapServiceResult(result);
    }

    [HttpPost("amount")]
    public async Task<IActionResult> VerifyDiscountCode([FromBody] GetFinalAmountDto dto) {


        var result = await _paymentService.CalculateAmount( dto.DiscountCode);

        return this.MapServiceResult(result);
    }

}