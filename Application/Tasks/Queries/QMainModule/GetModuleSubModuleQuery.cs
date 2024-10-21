using Domains.ViewModels;
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
    public class GetModuleSubModuleQuery : IRequest<HomeMenuVM>
    {
        public int RoleId { get; set; }
    }
    public class GetModuleSubModuleQueryHandler : IRequestHandler<GetModuleSubModuleQuery, HomeMenuVM>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetModuleSubModuleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<HomeMenuVM> Handle(GetModuleSubModuleQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.UserAccess.GetModuleSubModule(request.RoleId);
            return result;
        }
    }
}
