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
    public class GetFestivalBonusByIdQuery : IRequest<FestivalBonus>
    {
        public int FestivalBonusId { get; set; }
    }
    public class GetFestivalBonusByIdQueryHandler : IRequestHandler<GetFestivalBonusByIdQuery, FestivalBonus>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFestivalBonusByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FestivalBonus> Handle(GetFestivalBonusByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.GetFestivalBonusById(request.FestivalBonusId);
            return result;
        }
    }
}
