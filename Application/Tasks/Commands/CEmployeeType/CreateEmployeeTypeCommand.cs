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
    public class CreateEmployeeTypeCommand : IRequest<int>
    {
        public EmployeeType EmployeeType { get; set; }
    }

    public class CreateEmployeeTypeCommandHandler : IRequestHandler<CreateEmployeeTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.EmployeeTypes.Add(request.EmployeeType);
            return result;
        }
    }
}
