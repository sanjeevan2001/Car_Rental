using CarRental.Data;
using CarRental.Models;
using CarRental.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;

        public CustomerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Customer entity)
        {
            _appDbContext.Customers.Add(entity);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
