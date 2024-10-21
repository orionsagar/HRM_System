using Domains.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Commands.CTransactionLog
{
    public class CreateTransactionLogCommand : IRequest<int>
    {
        public string UserID { get; set; }
        public string TransectionID { get; set; }
        public string TransStatement { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string PageLink { get; set; }
        public string DocumentReferance { get; set; }
        public string CommandType { get; set; }
        public string PcName { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string CompID { get; set; }
        public DateTime EntryDate { get; set; }
    }

    public class TransactionLogCommandHandler : IRequestHandler<CreateTransactionLogCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHttpContextAccessor _accessor;

        public TransactionLogCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
        {
            this._unitOfWork = unitOfWork;
            _accessor = accessor;
        }

        public async Task<int> Handle(CreateTransactionLogCommand request, CancellationToken cancellationToken)
        {
            var userid = _accessor.HttpContext.Request.Cookies["UserID"];
            var comid = _accessor.HttpContext.Request.Cookies["CompID"];
            EditDeleteInfo log = new EditDeleteInfo
            {
                UserID = userid,
                ControllerName = request.ControllerName ?? _accessor.HttpContext.GetRouteValue("controller").ToString(),
                ActionName = request.ActionName ?? _accessor.HttpContext.GetRouteValue("action").ToString(),
                PageLink = _accessor.HttpContext.Request.Headers["Referer"],
                CommandType = request.CommandType,
                CompID = comid,
                DocumentReferance = request.DocumentReferance,
                IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                TransectionID = request.TransectionID,
                TransStatement = request.TransStatement,
                MacAddress = GlobalFunctions.GetMACAddress(),
                PcName = Environment.MachineName,
                EntryDate = DateTime.UtcNow,
            };

            await _unitOfWork.LogRepo.TransectionLog(log);
            return 1;
        }
    }
}
