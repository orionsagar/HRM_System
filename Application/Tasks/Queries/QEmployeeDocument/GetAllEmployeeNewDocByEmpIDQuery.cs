
using Application.Tasks.Queries.QEmployee;
using AutoMapper;
using Domains.Models;
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
    public class GetAllEmployeeNewDocByEmpIDQuery : IRequest<List<EmpDocumentsVM>>
    {
        public int EmpId { get; set; }
    }

    public class GetAllEmployeeNewDocByEmpIDHandler : IRequestHandler<GetAllEmployeeNewDocByEmpIDQuery, List<EmpDocumentsVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmployeeNewDocByEmpIDHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmpDocumentsVM>> Handle(GetAllEmployeeNewDocByEmpIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmpNewDocument.GetEmpAllDocsByEmpId(request.EmpId);
            return _mapper.Map<List<EmpDocumentsVM>>(result);
        }
    }
}
