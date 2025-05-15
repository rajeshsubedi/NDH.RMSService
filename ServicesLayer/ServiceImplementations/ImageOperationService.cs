using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DomainLayer.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ServicesLayer.ServiceInterfaces;

namespace ServicesLayer.ServiceImplementations
{
    public class ImageOperationService : IImageOperationService
    {
        private readonly Cloudinary _cloudinary;

        public ImageOperationService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new CustomInvalidOperationException("Cloudinary settings are not properly configured.");
            }

            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public async Task<string> SaveItemImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "Items", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<string> SaveCategoryImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "Categories", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }

        }


        public async Task<string> SaveSpecialGroupImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "HomePageSpecialGroup", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<string> SaveSpecialEventImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "HomePageSpecialEvent", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }
        }


        public async Task<string> SaveBannerImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "BannerImage", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<string> SaveCompanyDetailsImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new CustomInvalidOperationException("No image file provided.");

            // Uploading image to Cloudinary
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "CompanyDetail", // Cloudinary folder
                    PublicId = Guid.NewGuid().ToString(), // Unique ID for image
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                // Return the secure URL of the uploaded image
                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}
