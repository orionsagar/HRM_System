using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeDocuments
{
    public class EditEmployeeDocumentCommand : IRequest<int>
    {
        public EmployeeDocumentsVM EmployeeDocuments { get; set; }
    }
    public class EditEmployeeDocumentCommandHandler : IRequestHandler<EditEmployeeDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditEmployeeDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<int> Handle(EditEmployeeDocumentCommand request, CancellationToken cancellationToken)
        {
            var EmployeeDocuments = _mapper.Map<EmployeeDocuments>(request.EmployeeDocuments);
            var result = _unitOfWork._EmployeeDocument.Update(EmployeeDocuments);
            return result;
        }
    }
}
