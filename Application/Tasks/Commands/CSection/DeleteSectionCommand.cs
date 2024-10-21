using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CSection
{
    public class DeleteSectionCommand : IRequest<int>
    {
        public int SectionId { get; set; }
    }
    public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Sections.Delete(request.SectionId);
            return result;
        }
    }
}
