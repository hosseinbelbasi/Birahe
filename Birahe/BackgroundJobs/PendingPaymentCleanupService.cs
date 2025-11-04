using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.BackgroundJobs;

public class PendingPaymentCleanupService : BackgroundService {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingPaymentCleanupService> _logger;

    public PendingPaymentCleanupService(IServiceScopeFactory scopeFactory,
        ILogger<PendingPaymentCleanupService> logger) {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // run every 5 minutes
            try {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var userRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();

                var expiredTime = DateTime.UtcNow.AddMinutes(-20);

                var expiredPayments = await context.Payments
                    .Include(p => p.User)
                    .Where(p => p.Status == PaymentStatus.Pending && p.CreationDateTime < expiredTime)
                    .ToListAsync(stoppingToken);

                foreach (var payment in expiredPayments) {
                    payment.Status = PaymentStatus.Failed;
                    if (payment.User != null) {
                        await userRepo.LogicalDelete(payment.User.Id);
                    }
                }

                await context.SaveChangesAsync(stoppingToken);

                if (expiredPayments.Count > 0) {
                    _logger.LogInformation($"Cleaned up {expiredPayments.Count} expired pending payments.");
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error during pending payment cleanup.");
            }
        }
    }
}