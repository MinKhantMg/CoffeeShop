﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.Interface
{
    public interface IProductVariantRepository : IGenericRepository<ProductVariant, string>
    {
        Task<IEnumerable<ProductVariant>> GetAllIsDeletetedAsync();

        Task<ProductVariant> GetByIdAsync(string productVariantId);

        Task<IEnumerable<ProductVariant>> GetByProductIdAsync(string productId);
    }
}
