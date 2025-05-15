using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServicesLayer.ServiceInterfaces
{
    public interface IImageOperationService
    {
        Task<string> SaveItemImageAsync(IFormFile imageFile);
        Task<string> SaveCategoryImageAsync(IFormFile imageFile);
        Task<string> SaveSpecialEventImageAsync(IFormFile imageFile);
        Task<string> SaveBannerImageAsync(IFormFile imageFile);
        Task<string> SaveSpecialGroupImageAsync(IFormFile imageFile);
        Task<string> SaveCompanyDetailsImageAsync(IFormFile imageFile);
    }
}
