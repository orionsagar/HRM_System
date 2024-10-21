using Application.Tasks.Commands.CTransactionLog;
using Domains.Models;
using MediatR;
using Newtonsoft.Json;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static Dapper.SqlMapper;
using static Utility.Enums;

namespace Business.BonusNAllowance
{
    public interface IOtherAllowanceBL
    {
        Task<BLStatus> Upsert(List<OtherAllowance> entity);
        Task<BLStatus> Delete(int advaceId);
    }

    public class OtherAllowanceBL : IOtherAllowanceBL
    {
        private IUnitOfWork _unitOfWork;
        private IMediator _mediator;

        public OtherAllowanceBL(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<BLStatus> Upsert(List<OtherAllowance> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    if (item.IsFixed)
                    {
                        item.CalcRuleID = -1;
                    }
                    
                    if (item.CalcRuleID != -1)
                    {
                        item.Amount= await _unitOfWork.Rules.GetValueFromFunctionOfCalculationRule(item.CalcRuleID, item.Rate,
                                                                   item.EmpId, GlobalFunctions.FirstDayOfTheMonth(item.ApplyDate.Date), GlobalFunctions.LastDayOfTheMonth(item.ApplyDate.Date), item.CompId);
                    }
                    else
                    {
                        item.Amount = item.FixedAmount;
                    }
                   
                }

                await _unitOfWork.OtherAllowance.Upsert(entity);

                var dataJSON = JsonConvert.SerializeObject(entity);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = entity.FirstOrDefault().OtherAllowanceID.ToString(), CommandType = "Upsert", TransStatement = "Upsert Other Allowance", DocumentReferance = dataJSON });

                return new BLStatus { Message = "Successfully done. 😮😮" };
            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }

        public async Task<BLStatus> Delete(int OtherAllowanceID)
        {
            try
            {
                int count = await _unitOfWork.OtherAllowance.Delete(OtherAllowanceID);
                if (count > 0)
                {
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = OtherAllowanceID.ToString(), CommandType = "Delete", TransStatement = "Delete Other Allowance", DocumentReferance = OtherAllowanceID.ToString() });
                    return new BLStatus { Message = "Successfully deleted. 😮😮" };
                }
                else
                {
                    return new BLStatus { IsError = true, Message = "Data not found. 😐😐" };
                }

            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }


    }
}
