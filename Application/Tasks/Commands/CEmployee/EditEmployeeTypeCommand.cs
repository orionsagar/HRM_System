using Domains.Models;
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
    public class EditEmployeeCommand : IRequest<int>
    {
        public Employee Employee { get; set; }
    }
    public class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.Update(request.Employee);
            return result;
        }
    }
}
