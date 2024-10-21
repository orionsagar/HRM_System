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
    public class CreateSectionCommand : IRequest<int>
    {
        public Section Section { get; set; }
    }

    public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSectionCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<int> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Sections.Add(request.Section);
            return result;
        }
    }
}
