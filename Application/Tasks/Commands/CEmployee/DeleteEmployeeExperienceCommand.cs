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
    public class DeleteEmployeeExperienceCommand : IRequest<int>
    {
        public int EexpId { get; set; }
    }
    public class DeleteEmployeeExperienceCommandHandler : IRequestHandler<DeleteEmployeeExperienceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeExperienceCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteEmployeeExperienceCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.DeleteExperience(request.EexpId);
            return result;
        }
    }
}
