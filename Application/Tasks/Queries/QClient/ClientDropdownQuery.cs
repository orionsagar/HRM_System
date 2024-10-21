using Application.Tasks.Queries.QLeavePayMethod;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QClient
{
    public class ClientDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int ClientId { get; set; }
    }

    public class ClientDropdownQueryHandler : IRequestHandler<ClientDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(ClientDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ClientRepo.Dropdown();
            if(request.ClientId > 0)
            {
                result = await _unitOfWork.ClientRepo.Dropdown(request.ClientId);
            }
            return result.ToList();
        }
    }
}
