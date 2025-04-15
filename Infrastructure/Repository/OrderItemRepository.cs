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
    public class OrderItemRepository : GenericRepository<OrderItem, string>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext Context) : base(Context) { }

        public async Task<OrderItem> CreateOrderItem(OrderItem dto)
        {
            var query = "INSERT INTO OrderItems (Id, Quantity, Price, TotalPrice, Name, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedOn, DeletedBy, IsDeleted, Status, OrderId, ProductVariantId) " +
                        "VALUES (@Id, @Quantity, @Price, @TotalPrice, @Name, @CreatedBy, @CreatedOn, @LastModifiedBy, @LastModifiedOn, @DeletedOn, @DeletedBy, @IsDeleted, @Status, @OrderId, @ProductVariantId)";

            var result = await _connection.ExecuteAsync(query, dto);

            return result > 0 ? dto : null;
        }

    }
}
