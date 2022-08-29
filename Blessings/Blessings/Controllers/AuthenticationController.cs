using AutoMapper;
using Blessings.Data.Entities;
using Blessings.Models;
using Blessings.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blessings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticationService authenticationService, 
                                        IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authenticationService.SignInAsync(signInModel.Email, signInModel.Password);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailUsed = await _authenticationService.EmailUsedAsync(signUpModel.Email);

            if (emailUsed)
            {
                ModelState.AddModelError("error", "Email already exists");
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(signUpModel);

            await _authenticationService.AddUserAsync(user);

            return Ok();
        }
    }
}
