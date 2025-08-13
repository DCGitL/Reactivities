using System;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Email;

public interface IEmail : IEmailSender<User>
{
    Task SendEmailAsync(User user, string subject, string htmlMessage);
}
