using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeByEmpIdQuery : IRequest<Employee>
    {
        public int EmpId { get; set; }
    }

    public class GetEmployeeByEmpIdQueryHandler : IRequestHandler<GetEmployeeByEmpIdQuery, Employee>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByEmpIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Employee> Handle(GetEmployeeByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetAllByEmpId(request.EmpId);
            return _mapper.Map<Employee>(result);
        }
    }
}
