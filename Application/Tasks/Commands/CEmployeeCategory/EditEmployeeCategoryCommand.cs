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
    public class EditEmployeeCategoryCommand : IRequest<int>
    {
        public EmployeeCategory EmployeeCategory { get; set; }
    }

    public class EditEmployeeCategoryCommandHandler : IRequestHandler<EditEmployeeCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditEmployeeCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(EditEmployeeCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeCategorys.Update(request.EmployeeCategory);
            return result;
        }
    }
}
