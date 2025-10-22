using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase {
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService) {
        _paymentService = paymentService;
    }


    [HttpPost()]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentDto createPaymentDto) {
        var result = await _paymentService.CreatePaymentAsync(createPaymentDto);
        return this.MapServiceResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Verify([FromQuery] VerifyPaymentDto verifyPaymentDto) {
        var result = await _paymentService.VerifyPaymentAsync(verifyPaymentDto);
        return this.MapServiceResult(result);
    }

}