using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class PaymentRepository {
    private readonly ApplicationContext _context;

    public PaymentRepository(ApplicationContext context) {
        _context = context;
    }

    public async Task AddAsync(Payment payment) {
        await _context.Payments.AddAsync(payment);
    }

    public async Task<Payment?> GetByAuthorityAsync(string authority) {
        return await _context.Payments.FirstOrDefaultAsync(p => p.Authority == authority);
    }

    public void Update(Payment payment) {
        payment.ModificationDateTime = DateTime.UtcNow;
        _context.Payments.Update(payment);
    }

    public async Task<Discount?> FindDiscount(string key) {
        return await _context.Discounts.FirstOrDefaultAsync(d => d.Key == key);
    }
}