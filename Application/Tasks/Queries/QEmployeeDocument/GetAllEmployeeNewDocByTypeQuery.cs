
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
    public class GetAllEmployeeNewDocByTypeQuery : IRequest<EmpDocumentsVM>
    {
        public int EmpID {  get; set; }
        public string DocType { get; set; }
    }

    public class GetAllEmployeeNewDocByTypeHandler : IRequestHandler<GetAllEmployeeNewDocByTypeQuery, EmpDocumentsVM>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmployeeNewDocByTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmpDocumentsVM> Handle(GetAllEmployeeNewDocByTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmpNewDocument.GetEmpAllDocsByType(request.DocType, request.EmpID);
            return _mapper.Map<EmpDocumentsVM>(result);
        }
    }
}
