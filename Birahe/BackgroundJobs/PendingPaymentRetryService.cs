using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Birahe.EndPoint.Services;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.BackgroundJobs;

public class PendingPaymentRetryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingPaymentRetryService> _logger;

    public PendingPaymentRetryService(IServiceScopeFactory scopeFactory,
        ILogger<PendingPaymentRetryService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // run every 10 minutes
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();

                // find pending payments that are "old enough" but not expired
                var retryCutoff = DateTime.UtcNow.AddMinutes(-5); // e.g., give them a few minutes first
                var pendingPayments = await context.Payments
                    .Where(p => p.Status == PaymentStatus.Pending && p.CreationDateTime < retryCutoff)
                    .ToListAsync(stoppingToken);

                foreach (var payment in pendingPayments)
                {
                    try
                    {
                        var dto = new VerifyPaymentDto
                        {
                            Authority = payment.Authority,
                            Status = "OK" // assume still OK to verify; ZarinPal will tell us otherwise
                        };

                        var result = await paymentService.VerifyPaymentAsync(dto);

                        if (!result.Success)
                        {
                            _logger.LogWarning($"Payment {payment.Id} verification failed: {result.Message}");
                        }
                        else
                        {
                            _logger.LogInformation($"Payment {payment.Id} verification succeeded.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error verifying payment {payment.Id}");
                    }
                }

                if (pendingPayments.Count > 0)
                {
                    _logger.LogInformation($"Retried verification for {pendingPayments.Count} pending payments.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during pending payment retry job.");
            }
        }
    }
}