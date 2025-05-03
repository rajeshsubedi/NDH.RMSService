using DataAccessLayer.Infrastructure.Data;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
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

        public async Task AddHomepageSpecialGroupAsync(HomepageSpecialGroup homepageSpecialGroup)
        {
            _context.HomepageSpecialGroups.Add(homepageSpecialGroup);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HomepageSpecialGroup>> GetAllHomepageSpecialGroupsAsync()
        {
            return await _context.HomepageSpecialGroups.ToListAsync();
        }

        public async Task<HomepageSpecialGroup> GetHomepageSpecialGroupByNameAsync(string groupName)
        {
            return await _context.HomepageSpecialGroups
                .FirstOrDefaultAsync(sg => sg.GroupName == groupName);
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

        public async Task<List<BannerDetails>> GetAllBannerDetailsAsync()
        {
            return await _context.BannerDetails.ToListAsync();
        }

        public async Task AddBannerDetailsAsync(BannerDetails entity)
        {
            _context.BannerDetails.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CompanyDetails>> GetAllAGetAllCompanyDetailsAsyncsync()
        {
            return await _context.CompanyDetails.ToListAsync();
        }

        public async Task AddCompanyDetailsAsync(CompanyDetails entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<HomepageSpecialGroup> GetHomepageSpecialGroupByIdAsync(Guid id)
        {
            return await _context.HomepageSpecialGroups
                .FirstOrDefaultAsync(g => g.GroupId == id);
        }

        public async Task UpdateHomepageSpecialGroupAsync(HomepageSpecialGroup group)
        {
            _context.HomepageSpecialGroups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHomepageSpecialGroupAsync(HomepageSpecialGroup group)
        {
            _context.HomepageSpecialGroups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<SpecialEventDetails> GetSpecialEventByIdAsync(Guid id)
        {
            return await _context.SpecialEvents.FirstOrDefaultAsync(e => e.EventId == id);
        }
        public async Task<SpecialEventDetails> GetSpecialEventByNameAsync(string eventName)
        {
            return await _context.SpecialEvents.FirstOrDefaultAsync(e => e.EventName == eventName);
        }

        public async Task UpdateSpecialEventAsync(SpecialEventDetails specialEvent)
        {
            _context.SpecialEvents.Update(specialEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecialEventAsync(SpecialEventDetails specialEvent)
        {
            _context.SpecialEvents.Remove(specialEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<BannerDetails> GetBannerByIdAsync(Guid bannerId)
        {
            return await _context.BannerDetails.FirstOrDefaultAsync(b => b.BannerId == bannerId);
        }

        public async Task UpdateBannerAsync(BannerDetails banner)
        {
            _context.BannerDetails.Update(banner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBannerAsync(BannerDetails banner)
        {
            _context.BannerDetails.Remove(banner);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CompanyDetails>> GetAllCompanyDetailsAsync()
        {
            return await _context.CompanyDetails.ToListAsync();
        }

        public async Task<CompanyDetails> GetCompanyByIdAsync(Guid companyId)
        {
            return await _context.CompanyDetails.FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }

        public async Task UpdateCompanyAsync(CompanyDetails company)
        {
            _context.CompanyDetails.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(CompanyDetails company)
        {
            _context.CompanyDetails.Remove(company);
            await _context.SaveChangesAsync();
        }


    }
}
