using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeType
{
    public class EditEmployeeTypeCommand : IRequest<int>
    {
        public EmployeeType EmployeeType { get; set; }
    }
    public class EditEmployeeTypeCommandHandler : IRequestHandler<EditEmployeeTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditEmployeeTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(EditEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeTypes.Update(request.EmployeeType);
            return result;
        }
    }
}
