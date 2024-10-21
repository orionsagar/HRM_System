using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeDesignation
{
    public class CreateDesignationCommand : IRequest<int>
    {
        public Designation Designation { get; set; }
    }

    public class CreateSectionCommandHandler : IRequestHandler<CreateDesignationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Designations.Add(request.Designation);
            return result;
        }
    }
}
