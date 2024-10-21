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
    public class UpdateEmployeeHistoryStatusCommand : IRequest<int>
    {
        public EmployeeStatusVM EmployeeStatusVM { get; set; }
    }

    public class UpdateEmployeeHistoryStatusCommandHandler : IRequestHandler<UpdateEmployeeHistoryStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeHistoryStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(UpdateEmployeeHistoryStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.UpdateEmployeeHistoryStatus(request.EmployeeStatusVM);
            return result;
        }
    }
}
