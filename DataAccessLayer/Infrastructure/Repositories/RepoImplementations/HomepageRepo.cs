using DataAccessLayer.Infrastructure.Data;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.MenuManagementModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoImplementations
{
    public class HomePageRepo : IHomepageRepo
    {
        private readonly RMSServiceDbContext _context;

        public HomePageRepo(RMSServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync()
        {
            // Ensure proper querying and handling of nulls
            if (_context.MenuItems == null)
            {
                throw new InvalidOperationException("FoodItems DbSet is not initialized.");
            }

            return await _context.MenuItems
                .Where(item => item.IsSpecialOffer == true)
                .Select(item => new MenuItemDetails
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    DiscountPercentage = item.DiscountPercentage,
                    ImageUrl = item.ImageUrl,
                    OfferPeriod = item.OfferPeriod,
                    OfferDetails = item.OfferDetails,
                    IsSpecialOffer = item.IsSpecialOffer,
                    OrderLink = item.OrderLink,
                    CategoryId = item.CategoryId,
                    Category = new MenuCategoryDetails
                    {
                        CategoryId = item.Category.CategoryId,
                        Name = item.Category.Name,
                        Description = item.Category.Description,
                        ImageUrl = item.Category.ImageUrl
                    }
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync()
        {
            // Same verification and querying as above
            if (_context.SpecialEvents == null)
            {
                throw new InvalidOperationException("SpecialEvents DbSet is not initialized.");
            }

            return await _context.SpecialEvents.ToListAsync();
        }

        public async Task AddSpecialEventAsync(SpecialEventDetails specialEventDetails)
        {
            _context.SpecialEvents.Add(specialEventDetails);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description)
        {
            var query = _context.MenuItems.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(fi => fi.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(fi => fi.Description.Contains(description));
            }

            //if (minPrice.HasValue)
            //{
            //    query = query.Where(fi => fi.Price >= minPrice.Value);
            //}

            //if (maxPrice.HasValue)
            //{
            //    query = query.Where(fi => fi.Price <= maxPrice.Value);
            //}

            return await query.ToListAsync();
        }
    }
}
