using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QBreak
{
    public class GetBreakDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompId { get; set; }
    }
    public class GetBreakDropdownQueryHandler : IRequestHandler<GetBreakDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBreakDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetBreakDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Breaks.Dropdown(request.CompId);
            return result.ToList();
        }
    }
}
