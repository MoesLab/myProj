using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService

    {
        private readonly IConfiguration _configuration;
        public CloudMailService(IConfiguration configuration)
        {
            _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
        }

        public void Send(string subject, string message)
        {
            //send cloud mail output to debug window based on appsetting.json file
            Debug.WriteLine($"Mail from {_configuration["mailSettings:_mailFromAddress"]} to" +
                $" {_configuration["mailSettings:mailToAddress"]}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }

}

