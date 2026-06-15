using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Domain.Interfaces;
using PrintManager.Users.Infra.Context;
using PrintManager.Users.Infra.QueryDesigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintManager.Users.Infra.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IMongoCollection<Company> _companies;

        public CompanyRepository(MongoContext context)
        {
            _companies = context.Companies;
        }

        public async Task CreateAsync(Company company)
        {
            await _companies.InsertOneAsync(company);
        }

        public async Task<Company?> GetByIdAsync(string id)
        {
            return await _companies
                .Find(CompanyQueryDesigner.ById(id))
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _companies
                .Find(Builders<Company>.Filter.In(company => company.Id, ids))
                .ToListAsync();
        }
    }
}
