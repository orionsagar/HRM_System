using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScaleType
{
    public class GetPayScaleTypeDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetPayScaleTypeDropdownQueryHandler : IRequestHandler<GetPayScaleTypeDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetPayScaleTypeDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetPayScaleTypeDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.PayScaleTypeDropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
