using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeType
{
    public class DeleteEmployeeTypeCommand : IRequest<int>
    {
        public int EmpTypeId { get; set; }
    }
    public class DeleteEmployeeTypeCommandHandler : IRequestHandler<DeleteEmployeeTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeTypes.Delete(request.EmpTypeId);
            return result;
        }
    }
}
