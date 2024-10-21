using Domains.ViewModels;
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
    public class GetEmployeeListQuery : IRequest<List<EmployeeVM>>
    {
        //int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int ClientId { get; set; }
        public EmployeeFilterVM EmployeeFilterVM { get; set; }
    }
    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<EmployeeVM>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.GetEmployeeList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId, request.ClientId, request.EmployeeFilterVM);
            return result;
        }
    }
}
