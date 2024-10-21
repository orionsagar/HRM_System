using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QCompany
{
    public class GetCompanyDropdownQuery : IRequest<List<SelectListItemModel>>
    {
       
    }

    public class GetCompanyDropdownQueryHandler : IRequestHandler<GetCompanyDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetCompanyDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<List<SelectListItemModel>> Handle(GetCompanyDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.Dropdown();
            return result.ToList();
        }
    }
}
