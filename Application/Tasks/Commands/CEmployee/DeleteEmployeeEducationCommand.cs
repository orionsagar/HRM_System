using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployee
{
    public class DeleteEmployeeEducationCommand : IRequest<int>
    {
        public int EmpEduId { get; set; }
    }
    public class DeleteEmployeeEducationCommandHandler : IRequestHandler<DeleteEmployeeEducationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeEducationCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteEmployeeEducationCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.DeleteEducation(request.EmpEduId);
            return result;
        }
    }
}
