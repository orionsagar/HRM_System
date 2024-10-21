using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeCategory
{
    public class GetEmployeeCategoryByIdQuery : IRequest<EmployeeCategory>
    {
        public int EmpCatId { get; set; }
    }
    public class GetEmployeeCategoryByIdQueryHandler : IRequestHandler<GetEmployeeCategoryByIdQuery, EmployeeCategory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<EmployeeCategory> Handle(GetEmployeeCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeCategorys.GetById(request.EmpCatId);
            return result;
        }
    }
}
