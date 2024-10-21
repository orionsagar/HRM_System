using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CDepartment
{
    public class DeleteDepartmentCommand : IRequest<int>
    {
        public int DepartmentId { get; set; }
    }
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Departments.Delete(request.DepartmentId);
            return result;
        }
    }
}
