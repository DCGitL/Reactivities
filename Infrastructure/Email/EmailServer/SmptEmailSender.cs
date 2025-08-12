using System.Net.Mail;
using FluentEmail.Core.Models;

using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using EmailFluent = FluentEmail.Core;

namespace Infrastructure.EmailServer;

public class SmptEmailSender : ISmptEmailSender
{

    private readonly SmtpSender _sender;
    private const string from = "support@techtest.com";
    public SmptEmailSender(IConfiguration configuration)
    {
        _sender = new SmtpSender(() => new SmtpClient(host: "localhost")
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = configuration.GetValue<int>("EmailSettings:SmtpPort"),
            Host = configuration.GetSection("EmailSettings:SmtpHost").Value!
        });
    }

    public async Task<SendResponse> SendEmail(string to, string name, string subject, string body)
    {
        EmailFluent.Email.DefaultSender = _sender;



        var email = await EmailFluent.Email
         .From(from)
        .To(to, name)
        .Subject(subject)
        .Body(body, isHtml: true)
        .SendAsync();

        return email;

    }

}

