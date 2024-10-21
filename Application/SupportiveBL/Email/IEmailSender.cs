using Domains.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.SupportiveBL.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailUsingFile(string filePath, string subject, Dictionary<string, string> keyValuePairs, List<string> tos);
        Task<bool> SendEmailUsingFile(string filePath, string subject, Dictionary<string, string> keyValuePairs,List<DomainEventVM> domainEvent);
    }
}
