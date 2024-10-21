using Domains.Models;
using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Queries.QCompany
{
    public class GetCompanyByIdQuery : IRequest<CompanyViewModel>
    {
        public int CompID { get; set; }
    }
}
