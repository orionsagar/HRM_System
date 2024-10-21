using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domains.ViewModels.RecruitmentVM;

namespace Application.Tasks.Queries.QRecruitment
{
    public class SP_Dt_JobListQuery : IRequest<List<JobVM>>
    {
        public DataTableParamVM Param { get; set; }
    }
}
