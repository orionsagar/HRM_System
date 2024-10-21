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

namespace Application.Tasks.Queries.QFestivalBonus
{
    public class GetFestivalBonusReportQuery : IRequest<List<BonusEmpListVM>>
    {
        public BonusFilter FestivalBonusFilter { get; set; }
    }

    public class GetFestivalBonusReportQueryHandler : IRequestHandler<GetFestivalBonusReportQuery, List<BonusEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFestivalBonusReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BonusEmpListVM>> Handle(GetFestivalBonusReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.EmpFestivalBonusReport(request.FestivalBonusFilter);
            return result.ToList();
        }
    }
}
