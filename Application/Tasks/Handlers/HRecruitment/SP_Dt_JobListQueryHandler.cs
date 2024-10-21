using Application.Tasks.Queries.QRecruitment;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Domains.ViewModels.RecruitmentVM;

namespace Application.Tasks.Handlers.HRecruitment
{
    public class SP_Dt_JobListQueryHandler : IRequestHandler<SP_Dt_JobListQuery, List<JobVM>>
    {
        private readonly IUnitOfWork _work;

        public SP_Dt_JobListQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<JobVM>> Handle(SP_Dt_JobListQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.JobRepo.SP_Dt_JobList(request.Param);
            return result;
        }
    }
}
