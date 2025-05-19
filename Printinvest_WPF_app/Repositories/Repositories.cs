using Microsoft.EntityFrameworkCore;
using Printinvest_WPF_app.Contex;
using Printinvest_WPF_app.Models;
using Printinvest_WPF_app.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Printinvest_WPF_app.Repositories
{
   
   
    public static class RepositoryManager
    {
        private static AppDbContext _context;
        public static UserRepository Users { get; private set; }
        public static ProductRepository Products { get; private set; }
        public static ServiceRepository Services { get; private set; }
        public static AnalyticRepository Analytics { get; private set; }
        public static CartRepository Carts { get; private set; }
        public static CommentRepository Comments { get; private set; }
        public static OrderRepository Orders { get; private set; }

        public static void Initialize()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();

            Users = new UserRepository(_context);
            Products = new ProductRepository(_context);
            Services = new ServiceRepository(_context);
            Analytics = new AnalyticRepository(_context);
            Carts = new CartRepository(_context);
            Comments = new CommentRepository(_context);
            Orders = new OrderRepository(_context);

            var admin = Users.GetByLogin("admin");
            if (admin == null)
            {
                var adminUser = new User
                {
                    Login = "admin",
                    HashPassword = HashHelper.HashPassword("admin"),
                    Name = "Admin",
                    Role = UserRole.Admin
                };
                Users.Add(adminUser);
            }
        }
    }
}