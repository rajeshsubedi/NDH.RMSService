using DomainLayer.Models.DataModels.AuthenticationModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Models.DataModels.OrderManagementModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Data
{
    public class RMSServiceDbContext : DbContext
    {
        public DbSet<UserRegistrationDetails> UserRegistration { get; set; }
        public DbSet<MenuItemDetails> MenuItems { get; set; }
        public DbSet<MenuCategoryDetails> MenuCategories { get; set; }
        public DbSet<SpecialEventDetails> SpecialEvents { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderedItemsDetails> OrderItemDetails { get; set; }
        public DbSet<DeliveryAddressDetails> DeliveryAddressDetails { get; set; }
        public DbSet<PaymentOptionDetails> PaymentDetails { get; set; }



        private Guid id;
        public RMSServiceDbContext(DbContextOptions<RMSServiceDbContext> options) : base(options)
        {
            id = Guid.NewGuid();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegistrationDetails>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired().HasColumnType("uniqueidentifier");

                entity.Property(e => e.UserName).IsRequired().HasMaxLength(256).HasColumnType("nvarchar(256)");

                entity.Property(e => e.Email).IsRequired().HasColumnType("uniqueidentifier").HasMaxLength(256).HasColumnType("nvarchar(256)");

                entity.Property(e => e.PasswordHash).IsRequired().HasColumnType("nvarchar(max)");

                entity.Property(e => e.ResetPasswordOTP).HasColumnType("varchar(10)");

                entity.Property(e => e.OTPExpiration).HasColumnType("datetime2");

                entity.Property(e => e.Role).IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");

                entity.Property(e => e.EmailConfirmed).HasColumnType("bit");

                entity.Property(e => e.EmailConfirmToken).HasMaxLength(800).HasColumnType("uniqueidentifier").HasColumnType("nvarchar(800)");

                entity.Property(e => e.LastLogin).IsRequired().HasColumnType("datetime2");

                entity.Property(e => e.IsActive).IsRequired().HasColumnType("bit");

                entity.Property(e => e.SecurityStamp).HasMaxLength(128).HasColumnType("nvarchar(128)");

                entity.Property(e => e.PhoneNumber).IsRequired().HasColumnType("uniqueidentifier").HasMaxLength(20).HasColumnType("nvarchar(20)");

                entity.Property(e => e.PhoneNumberConfirmed).IsRequired().HasColumnType("bit");

                entity.Property(e => e.TwoFactorEnabled).IsRequired().HasColumnType("bit");

                entity.Property(e => e.AccessFailedCount).IsRequired().HasColumnType("int");

                entity.Property(e => e.RefreshToken).HasMaxLength(128).HasColumnType("nvarchar(128)");

                entity.Property(e => e.TokenExpiration).HasColumnType("datetime2");

                entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("datetime2");
            });


            // Configure FoodItemsDetails
            modelBuilder.Entity<MenuItemDetails>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.Property(e => e.ItemId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256).HasColumnType("nvarchar(256)");
                entity.Property(e => e.Description).HasMaxLength(500).HasColumnType("nvarchar(500)");
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.ImageUrl).HasColumnType("nvarchar(1024)");
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(1024)");
                entity.Property(e => e.OfferPeriod).HasColumnType("nvarchar(256)");
                entity.Property(e => e.OfferDetails).HasColumnType("nvarchar(500)");
                entity.Property(e => e.IsSpecialOffer).HasMaxLength(10).HasColumnType("bit");
                entity.Property(e => e.OrderLink).HasMaxLength(1024).HasColumnType("nvarchar(1024)");
                entity.Property(e => e.CategoryId).IsRequired().HasColumnType("uniqueidentifier");
            });


            // Configure FoodCategoriesDetails
            modelBuilder.Entity<MenuCategoryDetails>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256).HasColumnType("nvarchar(256)");
                entity.Property(e => e.Description).HasMaxLength(500).HasColumnType("nvarchar(500)");
                entity.Property(e => e.ImageUrl).HasMaxLength(1024).HasColumnType("nvarchar(1024)");
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(1024)");
                entity.HasMany(c => c.FoodItems)
                    .WithOne(e => e.Category)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<SpecialEventDetails>()
                .HasKey(e => e.EventId); // Configure primary key


            modelBuilder.Entity<OrderDetails>(entity => {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.UserId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.OrderDate).HasColumnType("datetime2");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderStatus).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.DeliveryDateTime).HasColumnType("datetime2");
                // Configuring the relationship with OrderedItemsDetails
                entity.HasMany(e => e.OrderedItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);

                // Configuring the relationship with PaymentOptionDetails
                entity.HasOne(o => o.PaymentOption)
                        .WithOne(po => po.Order)
                        .HasForeignKey<PaymentOptionDetails>(po => po.OrderId);

                // Configuring the relationship with DeliveryAddressDetails
                entity.HasOne(o => o.DeliveryAddress)
                        .WithOne(d => d.Order)
                        .HasForeignKey<DeliveryAddressDetails>(d => d.OrderId); // Foreign key in DeliveryAddressDetails
            });

            modelBuilder.Entity<OrderedItemsDetails>(entity => {
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.OrderItemId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.ItemName).IsRequired().HasColumnType("nvarchar(50)");
                entity.Property(e => e.OrderId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.FoodItemId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.Quantity).HasColumnType("int");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                // Configuring the relationship with FoodItemDetails
                entity.HasOne(e => e.FoodItem)
                      .WithMany()
                      .HasForeignKey(e => e.FoodItemId);

                // Configuring the relationship with OrderDetails
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderedItems)
                      .HasForeignKey(e => e.OrderId);
            });

            modelBuilder.Entity<PaymentOptionDetails>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.OrderId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.HasOne(e => e.Order)
                .WithOne(o => o.PaymentOption) // Updated to WithOne
                .HasForeignKey<PaymentOptionDetails>(e => e.OrderId); // Foreign key in PaymentOptionDetails
            });

            modelBuilder.Entity<DeliveryAddressDetails>(entity => {
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.AddressId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.OrderId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.UserId).IsRequired().HasColumnType("uniqueidentifier");
                entity.Property(e => e.StreetAddress).IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
                entity.Property(e => e.City).IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.State).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.ZipCode).HasMaxLength(20).HasColumnType("nvarchar(20)");
                entity.Property(e => e.Country).IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(e => e.IsDefault).IsRequired().HasColumnType("bit");
                // Configuring one-to-one relationship with OrderDetails
                entity.HasOne(e => e.Order)
                      .WithOne(o => o.DeliveryAddress)
                      .HasForeignKey<DeliveryAddressDetails>(e => e.OrderId);
            });


            base.OnModelCreating(modelBuilder);
            var initializer = new DbInitializer(modelBuilder);
            initializer.Seed();
        }

    }
}
