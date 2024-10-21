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
    public class EditDesignationCommand : IRequest<int>
    {
        public Designation Designation { get; set; }
    }

    public class EditSectionCommandHandler : IRequestHandler<EditDesignationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(EditDesignationCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Designations.Update(request.Designation);
            return result;
        }
    }
}
