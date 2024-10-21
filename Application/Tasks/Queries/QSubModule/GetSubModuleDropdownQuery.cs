using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSubModule
{
    public class GetSubModuleDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
    }
    public class GetSubModuleDropdownQueryHandler : IRequestHandler<GetSubModuleDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetSubModuleDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetSubModuleDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.SubModule.Dropdown(request.CompID);
            return result.ToList();
        }
    }
}
