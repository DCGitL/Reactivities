using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using API.DTOs;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;

namespace API.Controllers;

public class AccountController(SignInManager<User> signInManager,
 IEmailSender<User> emailSender,
IConfiguration configuration) : BaseApiController
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
        {
            await SendConFirmationEmailAsync(user, registerDto.Email);
            return Ok();
        }

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
    public async Task<IActionResult> Login(LoginDto loginDto, [FromQuery] bool useCookies, CancellationToken cancellationToken)
    {
        var errorCodeKey = "errorCode";
        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Detail = "NotAllowed",
            Title = "Unauthorized",
            Status = StatusCodes.Status401Unauthorized,
            Extensions = { [errorCodeKey] = "EmailNotConfirmed" }

        };

        if (useCookies)
        {
            var user = await signInManager.UserManager.FindByNameAsync(loginDto.Email);
            if (user is null)
            {
                problemDetails.Detail = $"User with email address {loginDto.Email} is unauthorized to use this resource";
                problemDetails.Extensions[errorCodeKey] = "Invalid email or password";
                return Unauthorized(problemDetails);
            }

            // var isUserLoggedIn = await userManager.CheckPasswordAsync(user, loginDto.Password);
            // if (!isUserLoggedIn)
            //     return Unauthorized("Invalid username or password");
            // //enforce email confirmation manually
            // if (!await userManager.IsEmailConfirmedAsync(user))
            // {
            //     return Unauthorized("Un confirmed email");
            //  }


            // Use Identity's built-in sign-in method for cookie authentication
            // await signInManager.SignInAsync(user, isPersistent: true);
            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    user.DisplayName,
                    user.Email,
                    user.Id,
                    user.ImageUrl
                });
            }

            if (result.IsNotAllowed)
            {

                return Unauthorized(problemDetails);

            }
            problemDetails.Detail = "Invalid Email or Password";

            return Unauthorized(problemDetails);


        }
        problemDetails.Detail = "Invalid login option";
        problemDetails.Extensions[errorCodeKey] = "you must use cookie authentication";
        return Unauthorized(problemDetails);

    }



    [AllowAnonymous]
    [HttpGet("resendConfirmEmail")]
    public async Task<IActionResult> ResendConfirmationEmail(string? email, string? userId)
    {

        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError(StatusCodes.Status400BadRequest.ToString(), "your email addres and userid cannot be empty");
            return ValidationProblem();
        }

        var user = await signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.Email == email || x.Id == userId);
        if (user is null || string.IsNullOrEmpty(user.Email))
        {
            ModelState.AddModelError(StatusCodes.Status400BadRequest.ToString(), "Ivalid Email");
            return ValidationProblem();
        }

        await SendConFirmationEmailAsync(user, user.Email);
        return Ok();

    }

    [AllowAnonymous]
    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail([Required] string userId, [Required] string code)
    {


        var user = await signInManager.UserManager.FindByIdAsync(userId);
        if (user is null)
        {

            return NotFound("User not found");
        }

        var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await signInManager.UserManager.ConfirmEmailAsync(user, decodedCode);
        if (result.Succeeded)
        {
            return Ok("Thank you for confirming your email");
        }

        ModelState.AddModelError(StatusCodes.Status400BadRequest.ToString(), "Email Confirmation Failed");
        return ValidationProblem();
    }
    private async Task SendConFirmationEmailAsync(User user, string email)
    {
        var code = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmEmailUrl = $"{configuration["ClientApUrl"]}/confirm-email?userId={user.Id}&code={code}";

        await emailSender.SendConfirmationLinkAsync(user, email, confirmEmailUrl);


    }


}
