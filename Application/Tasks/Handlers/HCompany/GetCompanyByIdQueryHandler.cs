using Application.Tasks.Queries.QCompany;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HCompany
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompanyViewModel> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetById(request.CompID);
            return _mapper.Map<CompanyViewModel>(result);
        }
    }
}
