using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScale
{
    public class GetPayScaleDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetPayScaleDropdownHandler : IRequestHandler<GetPayScaleDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetPayScaleDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetPayScaleDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
