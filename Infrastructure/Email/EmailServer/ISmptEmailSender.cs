using FluentEmail.Core.Models;

namespace Infrastructure.EmailServer;

public interface ISmptEmailSender
{
    Task<SendResponse> SendEmail(string to, string name, string subject, string body);
}
