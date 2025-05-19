using Microsoft.EntityFrameworkCore;
using Printinvest_WPF_app.Models;

namespace Printinvest_WPF_app.Contex
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Analytic> Analytics { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Properties.Settings.Default.DbConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.HashPassword)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Photo)
                .HasColumnType("VARBINARY(MAX)"); // Исправлено с BLOB на VARBINARY(MAX)

            // Конфигурация Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Photo)
                .HasColumnType("VARBINARY(MAX)"); // Добавлено для поддержки изображений

            // Конфигурация Service
            modelBuilder.Entity<Service>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Service>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired();
            modelBuilder.Entity<Service>()
                .Property(s => s.Photo)
                .HasColumnType("VARBINARY(MAX)"); // Добавлено для поддержки изображений

            // Конфигурация Analytic
            modelBuilder.Entity<Analytic>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Product)
                .WithMany()
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            modelBuilder.Entity<Analytic>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Конфигурация Cart
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация CartItem
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => ci.Id);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Service)
                .WithMany()
                .HasForeignKey(ci => ci.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Конфигурация Comment
            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>()
                .Property(c => c.Text)
                .IsRequired();

            // Конфигурация Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Service)
                .WithMany()
                .HasForeignKey(oi => oi.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}