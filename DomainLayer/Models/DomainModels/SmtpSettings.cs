using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DomainModels
{
    public class SmtpSettings
    {
        public string? AwsAccessKey { get; set; } // Nullable property
        public string? AwsSecretKey { get; set; } // Nullable property
        public string? AwsRegion { get; set; } // Nullable property
        public string? FromAddress { get; set; } // Nullable property
        public string? CompanyName { get; set; } // Nullable property
        public string? EmailVerificationApiUrl { get; set; } // Nullable property
        public string? EmailVerificationApiKey { get; set; } // Nullable property
    }

}
