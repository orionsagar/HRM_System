using Domains.Models;
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
    public class CreateDepartmentCommand : IRequest<int>
    {
        public Department Department { get; set; }
    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Departments.Add(request.Department);
            return result;
        }
    }
}
