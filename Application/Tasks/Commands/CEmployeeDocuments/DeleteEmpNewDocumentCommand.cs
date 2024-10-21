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
    public class DeleteEmpNewDocumentCommand : IRequest<int>
    {
        public int EmpDocId { get; set; }
    }
    public class DeleteEmpNewDocumentCommandHandler : IRequestHandler<DeleteEmpNewDocumentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteEmpNewDocumentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(DeleteEmpNewDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork._EmpNewDocument.Delete(request.EmpDocId);
            return result;
        }
    }
}
