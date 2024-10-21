using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetModuleDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
    }
    public class GetModuleDropdownQueryHandler : IRequestHandler<GetModuleDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetModuleDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<SelectListItemModel>> Handle(GetModuleDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.MainModules.Dropdown(request.CompID);
            return result.ToList();
        }
    }
}
