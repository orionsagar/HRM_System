using Domains.Models;
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
    public class EditSectionCommand : IRequest<int>
    {
        public Section Section { get; set; }
    }

    public class EditSectionCommandHandler : IRequestHandler<EditSectionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(EditSectionCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Sections.Update(request.Section);
            return result;
        }
    }
}
