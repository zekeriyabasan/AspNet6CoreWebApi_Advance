using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<UserForRegistrationDto,User>(userForRegistrationDto);
            var result = await _userManager.CreateAsync(user,userForRegistrationDto.Password);

            if (result.Succeeded) 
                await _userManager.AddToRolesAsync(user,userForRegistrationDto.Roles);

            return result;

        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAutnDto)
        {
            _user = await _userManager.FindByNameAsync(userForAutnDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user,userForAutnDto.Password));

            if (!result)
                _logger.LogWarning($"{nameof(ValidateUser)}  :  Authentication Failed. Username or Password Wrong!");

            return result;
        }
    }
}
