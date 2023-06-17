using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationManager:IAuthenticationService
    {
       private readonly ILoggerService _logger;
       private readonly IMapper _mapper;
       private readonly UserManager<User> _userManager;
       private readonly IConfiguration _configuration;

        private User? _user;

        public AuthenticationManager(ILoggerService logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAutnDto)
        {
            _user = await _userManager.FindByNameAsync(userForAutnDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAutnDto.Password));

            if (!result)
                _logger.LogWarning($"{nameof(ValidateUser)}  :  Authentication Failed. Username or Password Wrong!");

            return result;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigninCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigninCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken(
                issuer : jwtSettings["ValidIssuer"],
                audience : jwtSettings["ValidAudience"],
                claims:claims,
                expires : DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<UserForRegistrationDto,User>(userForRegistrationDto);
            var result = await _userManager.CreateAsync(user,userForRegistrationDto.Password);

            if (result.Succeeded) 
                await _userManager.AddToRolesAsync(user,userForRegistrationDto.Roles);

            return result;

        }
        
    }
}
