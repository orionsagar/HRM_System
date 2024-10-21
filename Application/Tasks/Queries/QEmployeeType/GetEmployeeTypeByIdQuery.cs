using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeType
{
    public class GetEmployeeTypeByIdQuery : IRequest<EmployeeType>
    {
        public int EmpTypeId { get; set; }
    }

    public class GetEmployeeTypeByIdQueryHandler : IRequestHandler<GetEmployeeTypeByIdQuery, EmployeeType>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeTypeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<EmployeeType> Handle(GetEmployeeTypeByIdQuery request, CancellationToken cancellationToken)
        {
           var result = _unitOfWork.EmployeeTypes.GetById(request.EmpTypeId);
            return result; 
        }
    }
}
