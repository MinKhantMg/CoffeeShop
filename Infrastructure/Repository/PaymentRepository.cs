using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Contracts;
using Domain.Database;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Repository
{
    public class PaymentRepository : GenericRepository<Payment, string>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext Context) : base(Context) { }

        public async Task<Payment> CreatePaymentAsync(Payment dto)
        {
            var query = "INSERT INTO Payments (Id, PaymentType, Amount, Name, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedOn, DeletedBy, IsDeleted, Status, OrderId) " +
                        "VALUES (@Id, @PaymentType, @Amount, @Name, @CreatedBy, @CreatedOn, @LastModifiedBy, @LastModifiedOn, @DeletedOn, @DeletedBy, @IsDeleted, @Status, @OrderId)";

            var result = await _connection.ExecuteAsync(query, dto);

            return result > 0 ? dto : null;
        }

    }
}
