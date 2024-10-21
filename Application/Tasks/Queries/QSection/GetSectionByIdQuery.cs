using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSection
{
    public class GetSectionByIdQuery : IRequest<Section>
    {
        public int SectionId { get; set; }
    }
    public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, Section>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSectionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public Task<Section> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Sections.GetById(request.SectionId);
            return result;
        }
    }
}
