using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CEmployeeDocuments
{
    public class EditEmpNewDocumentCommand : IRequest<int>
    {
        public EmpDocumentsVM EmployeeDocuments { get; set; }
    }
    public class EditEmpNewDocumentCommandHandler : IRequestHandler<EditEmpNewDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditEmpNewDocumentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<int> Handle(EditEmpNewDocumentCommand request, CancellationToken cancellationToken)
        {
            var EmployeeDocuments = _mapper.Map<EmpNewDocuments>(request.EmployeeDocuments);
            var result = _unitOfWork._EmpNewDocument.Update(EmployeeDocuments);
            return result;
        }
    }
}
