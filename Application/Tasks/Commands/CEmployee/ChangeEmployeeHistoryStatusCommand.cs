using Domains.Models;
using Domains.ViewModels;
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
    public class ChangeEmployeeHistoryStatusCommand : IRequest<int>
    {
        public List<EmployeeStatusVM> EmployeeStatusVM { get; set; }
    }

    public class ChangeEmployeeHistoryStatusCommandHandler : IRequestHandler<ChangeEmployeeHistoryStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeEmployeeHistoryStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(ChangeEmployeeHistoryStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.ChangeEmployeeHistoryStatus(request.EmployeeStatusVM);
            return result;
        }
    }
}
