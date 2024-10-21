using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QShift
{
    public class GetShiftDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }
    public class GetShiftDropdownQueryHandler : IRequestHandler<GetShiftDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetShiftDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetShiftDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Shifts.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
