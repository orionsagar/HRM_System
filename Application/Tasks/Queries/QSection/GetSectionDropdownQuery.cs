using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSection
{
    public class GetSectionDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetSectionDropdownHandler : IRequestHandler<GetSectionDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetSectionDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetSectionDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Sections.Dropdown(request.CompID,request.OrgId);
            return result.ToList();
        }
    }
}
