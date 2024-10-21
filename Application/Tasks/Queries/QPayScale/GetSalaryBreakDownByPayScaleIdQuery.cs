using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScale
{
    public class GetSalaryBreakDownByPayScaleIdQuery : IRequest<SalaryBreakDown>
    {
        public int PayScaleId { get; set; }
    }
    public class GetSalaryBreakDownByPayScaleIdQueryHandler : IRequestHandler<GetSalaryBreakDownByPayScaleIdQuery, SalaryBreakDown>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSalaryBreakDownByPayScaleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SalaryBreakDown> Handle(GetSalaryBreakDownByPayScaleIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.GetSalaryBreakDownByPayScaleId(request.PayScaleId);
            return result;
        }
    }
}
