using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Printinvest_WPF_app.Models
{
    public enum UserRole
    {
        Admin,
        Client,
        Manager
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public byte[] Photo { get; set; } // Опционально
        public List<Order> Orders { get; set; }
    }
    public interface IItem
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        byte[] Photo { get; }
    }
    public class Product : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public byte[] Photo { get; set; } // Опционально
    }

    public class Service : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Characteristics { get; set; }
        public byte[] Photo { get; set; } // Опционально
    }

    public class Analytic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
    }

    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int? ServiceId { get; set; }
        public Service Service { get; set; }
        private int _quantity;
        public int Quantity {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    Console.WriteLine($"CartItem Quantity changed to: {value}");
                }
            } 
        }

        public string ItemName => Product != null ? Product.Name : Service?.Name;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Order : INotifyPropertyChanged
    {
        private OrderStatus _status;

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public OrderStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public DateTime CreatedAt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public Product Product { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Completed,
        Cancelled
    }
}