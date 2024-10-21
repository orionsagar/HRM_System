using Application.Tasks.Commands.CEmployeeType;
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

namespace Application.Tasks.Commands.CEmployeeDocuments
{
    public class CreateEmpNewDocumentCommand : IRequest<int>
    {
        public EmpDocumentsVM EmployeeDocuments { get; set; }
    }

    public class CreateEmpNewDocumentCommandHandler : IRequestHandler<CreateEmpNewDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmpNewDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<int> Handle(CreateEmpNewDocumentCommand request, CancellationToken cancellationToken)
        {
            var EmployeeDocuments = _mapper.Map<EmpNewDocuments>(request.EmployeeDocuments);
            var result = _unitOfWork._EmpNewDocument.Add(EmployeeDocuments);
            return result;
        }
    }
}
