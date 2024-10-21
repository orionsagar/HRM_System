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
    public class CreateEmployeeDocumentCommand : IRequest<int>
    {
        public EmployeeDocumentsVM EmployeeDocuments { get; set; }
    }

    public class CreateEmployeeDocumentCommandHandler : IRequestHandler<CreateEmployeeDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmployeeDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<int> Handle(CreateEmployeeDocumentCommand request, CancellationToken cancellationToken)
        {
            var EmployeeDocuments = _mapper.Map<EmployeeDocuments>(request.EmployeeDocuments);
            var result = _unitOfWork._EmployeeDocument.Add(EmployeeDocuments);
            return result;
        }
    }
}
