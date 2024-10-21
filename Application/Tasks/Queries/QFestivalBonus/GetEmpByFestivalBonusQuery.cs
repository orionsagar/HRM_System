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
    public class GetEmpByFestivalBonusQuery : IRequest<List<BonusEmpListVM>>
    {
        public BonusFilter FestivalBonusFilter { get; set; }
    }

    public class GetEmpByFestivalBonusQueryHandler : IRequestHandler<GetEmpByFestivalBonusQuery, List<BonusEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByFestivalBonusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BonusEmpListVM>> Handle(GetEmpByFestivalBonusQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.EmpFestivalBonusList(request.FestivalBonusFilter);
            return result.ToList();
        }
    }
    public class GetEmployeeSearchFestivalBonusSetUpQuery : IRequest<List<BonusEmpListVM>>
    {
        public BonusFilter FestivalBonusFilter { get; set; }
    }

    public class EmployeeSearchFestivalBonusSetUpHandler : IRequestHandler<GetEmployeeSearchFestivalBonusSetUpQuery, List<BonusEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeSearchFestivalBonusSetUpHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BonusEmpListVM>> Handle(GetEmployeeSearchFestivalBonusSetUpQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.EmployeeSearchFestivalBonusSetUp(request.FestivalBonusFilter);
            return result.ToList();
        }
    }





    public class FestivalTypeDropdownQuery : IRequest<List<SelectListItemModel>>
    {
      
    }
    public class FestivalTypeDropdownQueryHandler : IRequestHandler<FestivalTypeDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FestivalTypeDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<SelectListItemModel>> Handle(FestivalTypeDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.FestivalTypeDropdown();
            return result.ToList();
        }
    }


    public class GetEmployeeByFestivalTypeQuery : IRequest<List<BonusEmpListVM>>
    {
        public int FestivalId { get; set; }
        public int BonusId { get; set; }
    }

    public class GetEmployeeByFestivalTypeQueryHandler : IRequestHandler<GetEmployeeByFestivalTypeQuery, List<BonusEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByFestivalTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BonusEmpListVM>> Handle(GetEmployeeByFestivalTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Bonus.GetEmployeeByFestivalTypeBonusId(request.FestivalId,request.BonusId);
            return result.ToList();
        }
    }


}
