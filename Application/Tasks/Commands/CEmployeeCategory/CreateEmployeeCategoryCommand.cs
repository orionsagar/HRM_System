using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeCategory
{
    public class CreateEmployeeCategoryCommand : IRequest<int>
    {
        public EmployeeCategory EmployeeCategory { get; set; }
    }

    public class CreateEmployeeCategoryCommandHandler : IRequestHandler<CreateEmployeeCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateEmployeeCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeCategorys.Add(request.EmployeeCategory);
            return result;
        }
    }
}
