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
    public class DeleteEmployeeCategoryCommand : IRequest<int>
    {
        public int EmpCatId { get; set; }
    }
    public class DeleteEmployeeCategoryCommandHandler : IRequestHandler<DeleteEmployeeCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteEmployeeCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeCategorys.Delete(request.EmpCatId);
            return result;
        }
    }
}
