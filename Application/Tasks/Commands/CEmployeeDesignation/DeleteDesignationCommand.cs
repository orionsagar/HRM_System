using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeDesignation
{
    public class DeleteDesignationCommand : IRequest<int>
    {
        public int DesignationId { get; set; }
    }
    public class DeleteSectionCommandHandler : IRequestHandler<DeleteDesignationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Designations.Delete(request.DesignationId);
            return result;
        }
    }
}
