using Application.Tasks.Queries.QTasks;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HTasks
{
    public class SP_Dt_TaskListQuryHandler : IRequestHandler<SP_Dt_TaskListQury, List<TasksVM>>
    {
        private readonly IUnitOfWork _work;

        public SP_Dt_TaskListQuryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<TasksVM>> Handle(SP_Dt_TaskListQury request, CancellationToken cancellationToken)
        {
            var task = await _work.TasksRepo.SP_Dt_TaskList(request.Param);
            return task.ToList();
        }
    }
}
