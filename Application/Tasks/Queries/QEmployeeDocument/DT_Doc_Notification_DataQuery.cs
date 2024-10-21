using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeDocument
{
    public class DT_Doc_Notification_DataQuery : IRequest<List<DocumentNotificationVM>>
    {
        public DataTableParamVM Param { get; set; }
    }

    public class DT_Doc_Notification_DataQueryHandler : IRequestHandler<DT_Doc_Notification_DataQuery, List<DocumentNotificationVM>>
    {
        private readonly IUnitOfWork _work;

        public DT_Doc_Notification_DataQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<DocumentNotificationVM>> Handle(DT_Doc_Notification_DataQuery request, CancellationToken cancellationToken)
        {
            var result = await _work._EmployeeDocument.SP_Dt_Doc_Notification_List(request.Param);
            return result;
        }
    }
}
