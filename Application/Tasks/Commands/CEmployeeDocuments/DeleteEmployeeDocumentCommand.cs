using Application.Tasks.Commands.CEmployeeType;
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
    public class DeleteEmployeeDocumentCommand : IRequest<int>
    {
        public int EmpDocId { get; set; }
    }
    public class DeleteEmployeeDocumentCommandHandler : IRequestHandler<DeleteEmployeeDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteEmployeeDocumentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(DeleteEmployeeDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork._EmployeeDocument.Delete(request.EmpDocId);
            return result;
        }
    }
}
