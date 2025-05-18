using Microsoft.EntityFrameworkCore;
using Printinvest_WPF_app.Contex;
using Printinvest_WPF_app.Models;
using Printinvest_WPF_app.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Printinvest_WPF_app.Repositories
{
    // Репозиторий для пользователей
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для продуктов
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для услуг
    public class ServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Service> GetAll()
        {
            return _context.Services.ToList();
        }

        public Service GetById(int id)
        {
            return _context.Services.Find(id);
        }

        public void Add(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для аналитики
    public class AnalyticRepository
    {
        private readonly AppDbContext _context;

        public AnalyticRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Analytic> GetAll()
        {
            return _context.Analytics.ToList();
        }

        public Analytic GetById(int id)
        {
            return _context.Analytics.Find(id);
        }

        public List<Analytic> GetByUserId(int userId)
        {
            return _context.Analytics.Where(a => a.UserId == userId).ToList();
        }

        public List<Analytic> GetByProductId(int productId)
        {
            return _context.Analytics.Where(a => a.ProductId == productId).ToList();
        }

        public List<Analytic> GetByServiceId(int serviceId)
        {
            return _context.Analytics.Where(a => a.ServiceId == serviceId).ToList();
        }

        public void Add(Analytic analytic)
        {
            _context.Analytics.Add(analytic);
            _context.SaveChanges();
        }

        public void Update(Analytic analytic)
        {
            _context.Analytics.Update(analytic);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var analytic = _context.Analytics.Find(id);
            if (analytic != null)
            {
                _context.Analytics.Remove(analytic);
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для корзин
    public class CartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public Cart GetByUserId(int userId)
        {
            return _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
        }

        public void Add(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void AddItemToCart(int cartId, CartItem item)
        {
            var cart = _context.Carts.Find(cartId);
            if (cart != null)
            {
                cart.Items.Add(item);
                RepositoryManager.Analytics.Add(
    new Analytic
    {
        ProductId = item.ProductId,
        ServiceId = item.ServiceId,
        Timestamp = DateTime.UtcNow,
        Action = "Cart",
        UserId = SessionManager.CurrentUser.Id,
    }
    );
                _context.SaveChanges();
            }
        }

        public void RemoveItemFromCart(int cartId, int itemId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == cartId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    _context.SaveChanges();
                }
            }
        }
    }

    // Репозиторий для комментариев
    public class CommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetAll()
        {
            return _context.Comments.Include(c => c.User).ToList();
        }

        public Comment GetById(int id)
        {
            return _context.Comments.Find(id);
        }

        public List<Comment> GetByProductId(int productId)
        {
            return _context.Comments.Include(c => c.User)
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.Timestamp)
                .Take(50)
                .ToList();
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для заказов
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