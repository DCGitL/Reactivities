using System;
using Domain;
using Infrastructure.EmailServer;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Email;

public class EmailSender(ISmptEmailSender smptEmailSender) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Confirm your email address";
        var body = $@"
            <p>Hi {user.DisplayName}</p>
            <p>Please confirm your email by clicking the link below</p>
            <p><a href='{confirmationLink}'> Click here to verify email</a> </p>
            <p>Thank you</p>
            ";

        var result = await smptEmailSender.SendEmail(user.Email!, user.DisplayName!, subject, body);
        var message = (result.Successful, result.ErrorMessages.Any()) switch
        {
            (true, false) => "Email sent successfuly",
            (false, true) => string.Format("error occured in sending your email {0}", string.Join(" ", result.ErrorMessages)),
            _ => "Unhandled error occured"
        };

        Console.WriteLine(message);

    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}
