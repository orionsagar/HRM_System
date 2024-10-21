using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeExperience
{
    public class GetEmployeeExperienceByEmpIdQuery : IRequest<List<EmployeeExperience>>
    {
        public int EmpId { get; set; }
    }

    public class GetEmployeeExperienceByEmpIdQueryHandler : IRequestHandler<GetEmployeeExperienceByEmpIdQuery, List<EmployeeExperience>>
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public GetEmployeeExperienceByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }

        public async Task<List<EmployeeExperience>> Handle(GetEmployeeExperienceByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeeExperienceByEmpId(request.EmpId);
            return result.ToList();
        }
    }
}
