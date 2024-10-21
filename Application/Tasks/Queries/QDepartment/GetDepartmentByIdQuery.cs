using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QDepartment
{
    public class GetDepartmentByIdQuery : IRequest<Department>
    {
        public int DepartmentId { get; set; }
    }
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Department>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<Department> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Departments.GetById(request.DepartmentId);
            return result;
        }
    }
}
