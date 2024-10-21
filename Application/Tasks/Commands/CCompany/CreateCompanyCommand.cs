using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Commands.CCompany
{
    public class CreateCompanyCommand : IRequest<int>
    {
        public CompanyViewModel CompanyViewModel { get; set; }
    }
}
