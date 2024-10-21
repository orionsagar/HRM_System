
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeDocument
{
    public class GetEmpAllDocsQuery : IRequest<List<EmpDocumentsVM>>
    {
        public int EmpDocId { get; set; }
        public int EmpId { get; set; }
    }
    public class GetEmpAllDocsQueryHandler : IRequestHandler<GetEmpAllDocsQuery, List<EmpDocumentsVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmpAllDocsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmpDocumentsVM>> Handle(GetEmpAllDocsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmpNewDocument.GetEmpAllDocs(request.EmpId, request.EmpDocId);
            return _mapper.Map<List<EmpDocumentsVM>>(result);
        }
    }
}
