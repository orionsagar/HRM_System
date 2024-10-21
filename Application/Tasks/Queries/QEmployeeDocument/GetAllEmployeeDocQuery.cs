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
    public class GetAllEmployeeDocQuery : IRequest<EmployeeDocumentsVM>
    {
        public int EmpDocId { get; set; }
    }

    public class GetAllEmployeeDocHandler : IRequestHandler<GetAllEmployeeDocQuery, EmployeeDocumentsVM>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmployeeDocHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDocumentsVM> Handle(GetAllEmployeeDocQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._EmpNewDocument.GetById(request.EmpDocId);
            return _mapper.Map<EmployeeDocumentsVM>(result);
        }
    }
}
