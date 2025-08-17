using System.ComponentModel.DataAnnotations;
using System.Text;
using API.DTOs;
using Domain;
using Infrastructure.Email;
using Infrastructure.SocialMedia.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.Controllers;

public class AccountController(SignInManager<User> signInManager,
 IEmail emailSender,
IConfiguration configuration, ILoginService loginService) : BaseApiController
{

    [AllowAnonymous]
    [HttpPost("github-login")]
    public async Task<IActionResult> LoginWithGithub(string code)
    {
        var result = await loginService.GitHubTokenResponse(code);
        if (result is null)
        {
            return BadRequest("Fail to login with github");
        }
        var user = await loginService.GetGitHubUser(result);
        if (user is null)
        {
            return BadRequest("Github user is not available for your request");
        }

        var existingUser = await signInManager.UserManager.FindByEmailAsync(user.Email);
        if (existingUser is null)
        {
            existingUser = new User
            {
                Email = user.Email,
                UserName = user.Email,
                DisplayName = user.Name,
                ImageUrl = user.ImageUrl
            };

            var createdResult = await signInManager.UserManager.CreateAsync(existingUser);
            if (!createdResult.Succeeded)
            {
                return BadRequest("Failed to create user");
            }
        }
        await signInManager.SignInAsync(existingUser, false);
        return Ok();

    }


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
             displayName =  user.DisplayName,
             email =   user.Email,
             id =   user.Id,
             imageUrl =   user.ImageUrl,
             isSocialLogin = string.IsNullOrEmpty(user.PasswordHash)
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
                  displayName =    user.DisplayName,
                  email =  user.Email,
                  id =   user.Id,
                  imageUrl =  user.ImageUrl,
                  isSocialLogin = string.IsNullOrEmpty(user.PasswordHash)
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


    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePassWordDto changePassWordDto)
    {
        var user = await signInManager.UserManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await signInManager.UserManager
        .ChangePasswordAsync(user, changePassWordDto.currentPassword, changePassWordDto.newPassword);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors.First().Description);

    }
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPassWordDto forgotPassWordDto)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(forgotPassWordDto.Email);
        if (user is null || !(await signInManager.UserManager.IsEmailConfirmedAsync(user)))
        {
            return BadRequest("Invalid email or email not confirmed.");
        }

        var tokenResult = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenResult));
        var callbackUrl = $"{configuration["ClientApUrl"]}/reset-password?token={token}&email={user.Email}";     // Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

        var htmlbody = $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>";
        await emailSender.SendEmailAsync(user, "Reset Password", htmlbody);

        return Ok("Password reset link sent");

    }

    [AllowAnonymous]

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPassWordDto model)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return BadRequest("Invalid Request");
        }

        var decodetoken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await signInManager.UserManager.ResetPasswordAsync(user, decodetoken, model.NewPassword);
        if (result.Succeeded)
        {
            return Ok("Password reset successful");
        }

        return BadRequest(result.Errors);

    }

    private async Task SendConFirmationEmailAsync(User user, string email)
    {
        var code = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmEmailUrl = $"{configuration["ClientApUrl"]}/confirm-email?userId={user.Id}&code={code}";

        await emailSender.SendConfirmationLinkAsync(user, email, confirmEmailUrl);


    }


}
