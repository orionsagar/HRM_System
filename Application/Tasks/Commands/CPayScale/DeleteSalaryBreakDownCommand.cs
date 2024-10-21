using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CPayScale
{
    public class DeleteSalaryBreakDownCommand : IRequest<int>
    {
        public int PayScaleID { get; set; }
    }
    public class DeleteSalaryBreakDownCommandHandler : IRequestHandler<DeleteSalaryBreakDownCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSalaryBreakDownCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(DeleteSalaryBreakDownCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
