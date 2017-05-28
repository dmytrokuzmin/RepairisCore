using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Repairis.Configuration;

namespace Repairis.Email
{
    public class EmailService : IEmailService, ISingletonDependency
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            if (!_emailSettings.IsEnabled)
            {
                return;
            }

            using (var client = new HttpClient { BaseAddress = new Uri(_emailSettings.ApiBaseUri) })
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes(_emailSettings.ApiKey)));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("from", _emailSettings.From),
                    new KeyValuePair<string, string>("to", to),
                    new KeyValuePair<string, string>("subject", subject),
                    new KeyValuePair<string, string>("html", message)
                });

                await client.PostAsync(_emailSettings.RequestUri,
                    content).ConfigureAwait(false);
            }
        }
    }
}
