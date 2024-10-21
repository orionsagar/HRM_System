using Application.Tasks.Queries.QEmployeeExperience;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeTrainingByEmpIdQuery : IRequest<List<EmployeeTraining>>
    {
        public int EmpId { get; set; }
    }

    public class GetEmployeeTrainingByEmpIdQueryHandler : IRequestHandler<GetEmployeeTrainingByEmpIdQuery, List<EmployeeTraining>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetEmployeeTrainingByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<List<EmployeeTraining>> Handle(GetEmployeeTrainingByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeeTrainingByEmpId(request.EmpId);
            return result.ToList();
        }
    }
}
