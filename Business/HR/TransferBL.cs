using Application.Tasks.Commands.CTransactionLog;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Newtonsoft.Json;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business.HR
{
    public interface ITransferBL
    {
        Task<BLStatus> Add(List<TransferVM> model);       
    }
    public class TransferBL : ITransferBL
    {
        private IUnitOfWork _unitOfWork;
        private IMediator _mediator;
        private IMapper _mapper;

        public TransferBL(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<BLStatus> Add(List<TransferVM> model)
        {
            List<EmployeeHistory> histories = new List<EmployeeHistory>();
            foreach (var item in model)
            {
                var empHistory = await _unitOfWork.Employees.GetEmpHistoryById(item.EmpHistoryId);
                if (empHistory != null)
                {
                    empHistory.EmpHistoryId = 0;

                    empHistory.SectId = item.SectId;
                    empHistory.DeptId = item.DeptId;                   
                    empHistory.EffectiveDate = item.EffectiveDate;
                    empHistory.DecisionDate = item.DecisionDate;
                    empHistory.TransType = (int)Enums.TransactionType.Transfer;
                    empHistory.Remarks = "Transfer: "+item.Remarks;
                    empHistory.AddedDate = item.AddedDate;
                    empHistory.AddedBy = item.AddedBy;
                    empHistory.UpdatedBy = null;
                    empHistory.UpdatedDate = null;

                    histories.Add(empHistory);
                }
            }

            var count = await _unitOfWork.Employees.Upsert(histories);
            if (count>0)
            {
                var dataJSON = JsonConvert.SerializeObject(histories);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(", ",model.Select(a=>a.EmpId)), CommandType = "Create", TransStatement = "Employee Transfer", DocumentReferance = dataJSON });

                return new BLStatus { Message = "Successfully Saved." };
            }
            else
            {
                return new BLStatus {IsError=true, Message = "No data for save." };
            }
        }

       


    }
}
