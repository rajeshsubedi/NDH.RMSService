using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DomainLayer.Exceptions;
using DomainLayer.Models.DomainModels;
using ServicesLayer.ServiceInterfaces;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime;
using Serilog;

namespace ServicesLayer.ServiceImplementations
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ISendGridClient _sendGridClient;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration configuration, ISendGridClient sendGridClient, HttpClient httpClient)
        {
            _smtpSettings = new SmtpSettings();
            configuration.GetSection("Smtp").Bind(_smtpSettings);
            _sendGridClient = sendGridClient;
            _httpClient = httpClient;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            if (!IsValidEmail(toEmail))
            {
                throw new ArgumentException("Invalid Email ID");
            }

            // Verify email existence
            //if (!await VerifyEmailAsync(toEmail))
            //{
            //    Log.Error("Email does not exist: {Email}", toEmail);
            //    throw new InvalidOperationException("Email does not exist.");
            //}

            var fromEmail = new EmailAddress(_smtpSettings.FromAddress, _smtpSettings.CompanyName);
            var toEmailAddress = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(fromEmail, toEmailAddress, subject, "", htmlContent);

            try
            {
                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    string responseBody = await response.Body.ReadAsStringAsync();
                    Log.Error("Failed to send email. Status code: {StatusCode}, Response: {ResponseBody}", response.StatusCode, responseBody);
                    throw new InvalidOperationException($"Failed to send email. Status code: {response.StatusCode}");
                }

                Log.Information("Email sent successfully to {Email}", toEmail);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Log.Error(ex, "Failed to send email.");
                throw new Exception("Failed to send email.", ex);
            }
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var requestUri = $"{_smtpSettings.EmailVerificationApiUrl}?api_key={_smtpSettings.EmailVerificationApiKey}&email={email}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseBody);
                    var status = jsonResponse["status"]?.ToString();
                    if (status == "valid")
                    {
                        return true;
                    }
                    else
                    {
                        Log.Error("Email verification status: {Status}", status);
                        return false;
                    }
                }
                else
                {
                    Log.Error("Failed to verify email. Status code: {StatusCode}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while verifying the email.");
                return false;
            }
        }


    }
}
