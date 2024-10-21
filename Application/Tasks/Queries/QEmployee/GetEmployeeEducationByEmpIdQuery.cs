using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeEducation
{
    public class GetEmployeeEducationByEmpIdQuery : IRequest<List<EmployeeEducation>>
    {
        public int EmpId { get; set; }
    }

    public class GetEmployeeEducationByEmpIdQueryHandler : IRequestHandler<GetEmployeeEducationByEmpIdQuery, List<EmployeeEducation>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeEducationByEmpIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeEducation>> Handle(GetEmployeeEducationByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeeEducationByEmpId(request.EmpId);
            return result.ToList();
        }
    }
}
