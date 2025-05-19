using Microsoft.EntityFrameworkCore;
using Printinvest_WPF_app.Contex;
using Printinvest_WPF_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printinvest_WPF_app.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.Include(o => o.Items).ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetByUserId(int userId)
        {
            return _context.Orders.Include(o => o.Items).Where(o => o.UserId == userId).ToList();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }

    // Статический менеджер репозиториев
}
