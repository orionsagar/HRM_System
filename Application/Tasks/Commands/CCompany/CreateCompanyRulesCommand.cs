using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Tasks.Commands.CCompany
{
    public class CreateCompanyRulesCommand : IRequest<int>
    {
        public List<CompanyRule> CompanyRules { get; set; }
        public int CompId { get; set; }
    }

    public class CreateCompanyRulesCommandHandler : IRequestHandler<CreateCompanyRulesCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CreateCompanyRulesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCompanyRulesCommand request, CancellationToken cancellationToken)
        {            
            var result = await _unitOfWork.Companys.AddCompanyRule(request.CompanyRules, request.CompId);
            return result;
        }
    }
}
