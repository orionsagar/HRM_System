using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeEducation
{
    public class GetEmployeeEducationByIdQuery : IRequest<EmployeeEducation>
    {
        public int EmpEducationId { get; set; }
    }

    public class GetEmployeeEducationByIdQueryHandler : IRequestHandler<GetEmployeeEducationByIdQuery, EmployeeEducation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeEducationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeEducation> Handle(GetEmployeeEducationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeeEducationById(request.EmpEducationId);
            return _mapper.Map<EmployeeEducation>(result);
        }
    }
}
