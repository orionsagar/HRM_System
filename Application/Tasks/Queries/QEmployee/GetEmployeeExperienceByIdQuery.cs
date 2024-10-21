using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeExperience
{
    public class GetEmployeeExperienceByIdQuery : IRequest<EmployeeExperience>
    {
        public int EmpExperienceId { get; set; }
    }

    public class GetEmployeeExperienceByIdQueryHandler : IRequestHandler<GetEmployeeExperienceByIdQuery, EmployeeExperience>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeExperienceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeExperience> Handle(GetEmployeeExperienceByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmployeeExperienceById(request.EmpExperienceId);
            return _mapper.Map<EmployeeExperience>(result);
        }
    }
}
