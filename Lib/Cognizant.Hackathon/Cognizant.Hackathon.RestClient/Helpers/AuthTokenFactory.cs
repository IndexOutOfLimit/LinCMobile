using System;
using System.Text;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public class AuthTokenFactory
    {
        public static AuthToken CreateBasicAuthToken(string username, string password, long ttl)
        {
            var accessToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            var issuedAt = DateTime.UtcNow;

            return new AuthToken
            {
                IssuedAt = issuedAt,
                TokenType = "Basic",
                AccessToken = accessToken,
                ExpiresIn = ttl,
                ExpiresAt = issuedAt.AddSeconds(ttl)
            };
        }
    }
}
