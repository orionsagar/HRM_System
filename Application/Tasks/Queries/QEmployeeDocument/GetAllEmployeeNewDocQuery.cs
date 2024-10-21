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
    public class GetAllEmployeeNewDocQuery : IRequest<EmpDocumentsVM>
    {
        public int EmpDocId { get; set; }
    }

    public class GetAllEmployeeNewDocHandler : IRequestHandler<GetAllEmployeeNewDocQuery, EmpDocumentsVM>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmployeeNewDocHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmpDocumentsVM> Handle(GetAllEmployeeNewDocQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmpNewDocument.GetById(request.EmpDocId);
            return _mapper.Map<EmpDocumentsVM>(result);
        }
    }
}
