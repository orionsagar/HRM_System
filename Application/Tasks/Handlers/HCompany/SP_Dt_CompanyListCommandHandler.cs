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

namespace Application.Tasks.Handlers.HCompany
{    
    public class SP_Dt_CompanyListQuery : IRequest<List<CompanyViewModel>>
    {
        public int DisplayLength { get; set; }
        public int Start { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
    }
    public class SP_Dt_CompanyListCommandHandler : IRequestHandler<SP_Dt_CompanyListQuery, List<CompanyViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SP_Dt_CompanyListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<CompanyViewModel>> Handle(SP_Dt_CompanyListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.SP_Dt_CompanyList(request.DisplayLength, request.Start, request.SortCol, request.SortDir, request.Search);
            //var data = _mapper.Map<List<CompanyViewModel>>(result);
            return result.ToList();
        }
    }
}
