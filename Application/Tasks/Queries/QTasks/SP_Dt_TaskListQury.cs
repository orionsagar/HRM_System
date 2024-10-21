using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QTasks
{
    public class SP_Dt_TaskListQury : IRequest<List<TasksVM>>
    {
        public DataTableParamVM Param { get; set; }
    }
}
