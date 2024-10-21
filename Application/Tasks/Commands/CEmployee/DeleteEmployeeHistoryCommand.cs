using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployee
{
    public class DeleteEmployeeHistoryCommand : IRequest<int>
    {
        public List<int> EmpHistorysId { get; set; }
    }
    public class DeleteEmployeeHistoryCommandHandler : IRequestHandler<DeleteEmployeeHistoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeHistoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(DeleteEmployeeHistoryCommand request, CancellationToken cancellationToken)
        {
            var result =await _unitOfWork.Employees.DeleteEmpHistory(request.EmpHistorysId);
            return result;
        }
    }
}
