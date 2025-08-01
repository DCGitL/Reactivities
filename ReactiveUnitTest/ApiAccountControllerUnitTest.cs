using System;
using API.Controllers;
using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;

namespace ReactiveUnitTest;

public class ApiAccountControllerUnitTest
{
    private UserManager<User> _subUserManager;
    private SignInManager<User> _subSignInManager;
    private AccountController _acctController;
    public ApiAccountControllerUnitTest()
    {

        _subUserManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _subSignInManager = Substitute.For<SignInManager<User>>(_subUserManager,
                            Substitute.For<IHttpContextAccessor>(),
                            Substitute.For<IUserClaimsPrincipalFactory<User>>(),
                            null, null, null, null);
        _acctController = new AccountController(_subSignInManager);

    }


    [Fact]
    public async Task RegisterUserAndSignIn_SuccessfulCreationAndSignIn_ReturnsOK()
    {
        //Arrange
        var registerDto = new RegisterDto
        {
            DisplayName = "bob",
            Email = "bob@test.com",
            Password = "test!QAZ12"
        };
        //substitute UserManager.CreateAsync to return success
        _subUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);

        //substitute the SigninAsync to complete succeddfully
        _subSignInManager.SignInAsync(Arg.Any<User>(), Arg.Any<bool>(), Arg.Any<string>()).Returns(Task.CompletedTask);

        //Act 
        var result = await _acctController.RegisterUser(registerDto);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);

    }

    [Fact]
    public async Task RegisterUserAndSignIn_FailCreationAndSignIn_ReturnsProblem()
    {
        //Arrange
        var registerDto = new RegisterDto
        {
            DisplayName = "bob",
            Email = "bob@test",
            Password = "test!QAZ12"
        };
        IdentityError[] identityErrors = {
            new IdentityError
            {
                Code = StatusCodes.Status400BadRequest.ToString(),
                Description = $"Invalid email {registerDto.Email}"
            },
         };
        //substitute UserManager.CreateAsync to return success
        _subUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Failed(identityErrors));

        //substitute the SigninAsync to complete succeddfully
        _subSignInManager.SignInAsync(Arg.Any<User>(), Arg.Any<bool>(), Arg.Any<string>()).Returns(Task.CompletedTask);

        //Act 
        var result = await _acctController.RegisterUser(registerDto);

        //Assert
        Assert.NotNull(result);
        var value = Assert.IsType<ObjectResult>(result).Value as ValidationProblemDetails;

        var code = value!.Errors.Keys.First();
        Assert.Equal(code, identityErrors.First().Code);

        var description = value!.Errors[code].First();
        Assert.Equal(description, identityErrors.First().Description);

    }

}
