using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store_entities;
using Store_entities.Entities;

namespace Store.Services
{
    public class ProviderService : IProviderService
    {
        private readonly StoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProviderService(StoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateProvider(string providerName)
        {
            var providerEntity = new ProviderEntity
            {
                Name = providerName
            };
            _dbContext.Providers.Add(providerEntity);
            await _dbContext.SaveChangesAsync();
            return providerEntity.Id;
        }

        public async Task<List<Provider>> GetProviders()
        {
            var providers = await _dbContext.Providers
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<Provider>>(providers);
        }

        public async Task<Provider> GetProviderById(int id)
        {
            var provider = await _dbContext.Providers
                .FindAsync(id);
            if (provider == null)
            {
                throw new ArgumentException("Not found");
            }

            return _mapper.Map<Provider>(provider);
        }
    }
}