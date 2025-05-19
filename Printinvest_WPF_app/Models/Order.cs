using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printinvest_WPF_app.Models
{
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
