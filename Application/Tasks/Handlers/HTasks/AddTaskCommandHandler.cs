using Application.Tasks.Commands.CTasks;
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
    public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, int>
    {
        private readonly IUnitOfWork _work;

        public AddTaskCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            return await _work.TasksRepo.Add(request.Tasks);
        }
    }
}
