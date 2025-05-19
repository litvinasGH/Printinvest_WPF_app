using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
