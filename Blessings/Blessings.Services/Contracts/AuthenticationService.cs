using Blessings.Data;
using Blessings.Data.Entities;
using Blessings.Services.Impl;
using Blessings.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blessings.Services.Contracts
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JwtTokenOptions _tokenOptions;
        public AuthenticationService(ApplicationDbContext applicationDbContext,
                                     IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _applicationDbContext = applicationDbContext;
            _tokenOptions = jwtTokenOptions.Value;
        }

        public Task<bool> EmailUsedAsync(string email) =>
             _applicationDbContext.Users.AnyAsync(x => x.Email == email);


        public Task<User> GetUserByIdAsync(int id) =>
             _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);


        public async Task AddUserAsync(User user)
        {
            user.RegistrationDate = DateTime.UtcNow;
            _applicationDbContext.Users.Add(user);
            await _applicationDbContext.SaveChangesAsync();
        }

        public Task<User> GetUserByEmailAndPasswordAsync(string email, string password) =>
              _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

        public async Task<string> SignInAsync(string email, string password)
        {
            var user = await GetUserByEmailAndPasswordAsync(email, password);

            var usersClaims = new[]
           {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            return GenerateAccessToken(usersClaims);
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(_tokenOptions.Secret);
            var expirationTime = DateTime.UtcNow.AddMonths(_tokenOptions.AccessTokenDurationInMinutesRememberMe);
            var jwtToken = new JwtSecurityToken(issuer: "Blinkingcaret",
                                                audience: "Anyone",
                                                claims: claims,
                                                notBefore: DateTime.UtcNow,
                                                expires: expirationTime,
                                                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                                                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}
