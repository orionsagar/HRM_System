//using Domains.ViewModels;
//using MediatR;
//using Persistence.DAL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Application.Tasks.Queries.QEmployeeType
//{
//    public class SP_Dt_EmpTypeListQuery : IRequest<List<SP_Dt_EmpTypeList>>
//    {
//        public int DisplayLength { get; set; }
//        public int DisplayStart { get; set; }
//        public int SortCol { get; set; }
//        public string SortDir { get; set; }
//        public string Search { get; set; }
//        public int ComId { get; set; }
//        public int OrgId { get; set; }
//    }
//    public class SP_Dt_EmpTypeListQueryHandler : IRequestHandler<SP_Dt_EmpTypeListQuery, List<SP_Dt_EmpTypeList>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public SP_Dt_EmpTypeListQueryHandler(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public async Task<List<SP_Dt_EmpTypeList>> Handle(SP_Dt_EmpTypeListQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _unitOfWork.EmployeeTypes.SP_Dt_EmpTypeList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search, request.ComId,request.OrgId);
//            return result;
//        }
//    }
//}



using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeType
{
    public class SP_Dt_EmpTypeListQuery : IRequest<List<SP_Dt_EmpTypeList>>
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public int ComId { get; set; }
        public int OrgId { get; set; }
    }
    public class SP_Dt_EmpTypeListQueryHandler : IRequestHandler<SP_Dt_EmpTypeListQuery, List<SP_Dt_EmpTypeList>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_Dt_EmpTypeListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<SP_Dt_EmpTypeList>> Handle(SP_Dt_EmpTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeTypes.SP_Dt_EmpTypeList(request.DisplayLength, request.DisplayStart, request.SortCol, request.SortDir, request.Search,request.ComId ,request.OrgId);
            return result;
        }
    }
}









