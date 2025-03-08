using DomainLayer.Models.DataModels.AuthenticationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            string categoryAppitizerIdString = "9D1F88A2-C1F5-46F2-9C7E-6E035434EF2E";
            string categoryMainCourseIdString = "B9AD857D-70FD-4BA6-A845-7865FD509AE6";
            string categoryDesertIdString = "33D92049-53E9-404B-9979-9340F0C579B1";

            //_modelBuilder.Entity<UserRegistrationDetails>().HasData(
            //    new UserRegistrationDetails
            //    {
            //        UserId = Guid.NewGuid(),
            //        UserName = "Rajesh Subedi",
            //        PasswordHash = "$2a$11$13DY.QCK5GdM52lr.fShoOGrg366Z3YbLEc8oPpd0pyF6XMCAjN1K", // Example hash for 'password'
            //        Role = "user",
            //        Email = "artsrazes@gmail.com",
            //        EmailConfirmToken = "ac33895a-1f42-463e-9feb-9753abf068e8",
            //        IsActive = true,
            //        CreatedAt = DateTime.UtcNow,
            //        LastLogin = DateTime.UtcNow,
            //        EmailConfirmed = false,
            //        SecurityStamp = Guid.NewGuid().ToString(),
            //        PhoneNumber = "9862541725",
            //        PhoneNumberConfirmed = true,
            //        TwoFactorEnabled = false,
            //        AccessFailedCount = 0
            //    }
            //);

            //// Seed data for FoodCategories
            //_modelBuilder.Entity<FoodCategoriesDetails>().HasData(
            //    new FoodCategoriesDetails
            //    {
            //        CategoryId = Guid.NewGuid(),
            //        Name = "Appetizers",
            //        Description = "Starters to begin your meal",
            //        ImageUrl = "/images/appetizers.jpg"
            //    },
            //    new FoodCategoriesDetails
            //    {
            //        CategoryId = Guid.NewGuid(),
            //        Name = "Main Course",
            //        Description = "Hearty main dishes to fill you up",
            //        ImageUrl = "/images/main_course.jpg"
            //    },
            //    new FoodCategoriesDetails
            //    {
            //        CategoryId = Guid.NewGuid(),
            //        Name = "Desserts",
            //        Description = "Sweet treats to end your meal",
            //        ImageUrl = "/images/desserts.jpg"
            //    }
            //);


           // //Seed data for FoodItems
           //_modelBuilder.Entity<FoodItemsDetails>().HasData(
           //    new FoodItemsDetails
           //    {
           //        ItemId = Guid.NewGuid(),
           //        Name = "Spring Rolls",
           //        Description = "Crispy rolls filled with vegetables",
           //        Price = 5.99m,
           //        DiscountPercentage = 10,
           //        ImageUrl = "/images/spring_rolls.jpg",
           //        OfferPeriod = "All day",
           //        OfferDetails = "10% off",
           //        OrderLink = "/order/spring-rolls",
           //        CategoryId = Guid.Parse(categoryAppitizerIdString)


           //    },
           //    new FoodItemsDetails
           //    {
           //        ItemId = Guid.NewGuid(),
           //        Name = "Grilled Chicken",
           //        Description = "Juicy grilled chicken with herbs",
           //        Price = 12.99m,
           //        DiscountPercentage = 15,
           //        ImageUrl = "/images/grilled_chicken.jpg",
           //        OfferPeriod = "Dinner",
           //        OfferDetails = "15% off",
           //        OrderLink = "/order/grilled-chicken",
           //        CategoryId = Guid.Parse(categoryMainCourseIdString)


           //    },
           //    new FoodItemsDetails
           //    {
           //        ItemId = Guid.NewGuid(),
           //        Name = "Chocolate Cake",
           //        Description = "Rich chocolate cake with frosting",
           //        Price = 6.99m,
           //        DiscountPercentage = 5,
           //        ImageUrl = "/images/chocolate_cake.jpg",
           //        OfferPeriod = "After 6 PM",
           //        OfferDetails = "5% off",
           //        OrderLink = "/order/chocolate-cake",
           //        CategoryId = Guid.Parse(categoryDesertIdString)

           //    }
           //);
        }
    }

}
