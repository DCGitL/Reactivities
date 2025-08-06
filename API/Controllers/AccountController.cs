using System;
using System.Security.Claims;
using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName

        };
        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
            return Ok();

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);

        }
        return ValidationProblem();
    }


    [AllowAnonymous]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false)
            return Unauthorized("User is not Logged in");

        var user = await signInManager.UserManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        return Ok(new
        {
            user.DisplayName,
            user.Email,
            user.Id,
            user.ImageUrl
        });

    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDto loginDto, [FromQuery]bool useCookies, CancellationToken cancellationToken)
    {
        if (useCookies)
        {
            var user = await userManager.FindByNameAsync(loginDto.Email);
            if (user is null)
                return Unauthorized($"User with email address {loginDto.Email} is unauthorized to use this resource");

            var isUserLoggedIn = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isUserLoggedIn)
                return Unauthorized("Invalid password");

            // Use Identity's built-in sign-in method for cookie authentication
            await signInManager.SignInAsync(user, isPersistent: true);

            return Ok("Logged in");
        }
        else
            return Unauthorized("Invalid login options");

      


    }

}
