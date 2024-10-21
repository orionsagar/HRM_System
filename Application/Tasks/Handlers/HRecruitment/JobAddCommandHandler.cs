using Application.Tasks.Commands.CRecruitment;
using AutoMapper;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Domains.Models.Recruitment;

namespace Application.Tasks.Handlers.HRecruitment
{
    public class JobAddCommandHandler : IRequestHandler<JobAddCommand, int>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public JobAddCommandHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<int> Handle(JobAddCommand request, CancellationToken cancellationToken)
        {
            var data = _mapper.Map<Job>(request.JobVM);
            var result = await _work.JobRepo.Add(data);
            return result;
        }
    }
}
