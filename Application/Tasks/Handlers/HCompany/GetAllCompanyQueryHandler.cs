using Application.Tasks.Queries.QCompany;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HCompany
{
    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, List<Company>>
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public GetAllCompanyQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public async Task<List<Company>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetAll();
            return result.ToList();
        }
    }
}
