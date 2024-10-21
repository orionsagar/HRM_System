using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeDesignation
{
    public class GetDesignationByIdQuery : IRequest<Designation>
    {
        public int DesignationId { get; set; }
    }
    public class GetDesignationByIdQueryHandler : IRequestHandler<GetDesignationByIdQuery, Designation>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDesignationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Designation> Handle(GetDesignationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Designations.GetById(request.DesignationId);
            return result;
        }
    }
}
