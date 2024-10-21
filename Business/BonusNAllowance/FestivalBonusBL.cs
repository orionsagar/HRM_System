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
using static Utility.Enums;

namespace Business.BonusNAllowance
{
    public interface IFestivalBonusBL
    {
        Task<BLStatus> Upsert(List<FestivalBonus> entity);
        Task<BLStatus> Upsert(List<FestivalBonusSetup> entity);
        Task<BLStatus> Upsert(FestivalType entity);
        Task<BLStatus> Delete(int festBounusId);
        Task<BLStatus> DeleteBonusSetUp(int bounusId);
    }

    public class FestivalBonusBL : IFestivalBonusBL
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public FestivalBonusBL(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<BLStatus> Upsert(List<FestivalBonus> entity)
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
                        item.Amount = await _unitOfWork.Rules.GetValueFromFunctionOfCalculationRule(item.CalcRuleID, item.Rate,
                                                                   item.EmpId, GlobalFunctions.FirstDayOfTheMonth(item.ApplyDate.Date), GlobalFunctions.LastDayOfTheMonth(item.ApplyDate.Date), item.CompId);
                    }
                    else
                    {
                        item.Amount = item.FixedAmount;
                    }

                }

                await _unitOfWork.Bonus.Upsert(entity);

                var data = JsonConvert.SerializeObject(entity);
                await _mediator.Send(new CreateTransactionLogCommand() { TransectionID = string.Join(",", entity.Select(f=>f.FestivalBonusID)),CommandType = "Upsert",TransStatement = "Upsert Festival Bonus", DocumentReferance = data });

                return new BLStatus { Message = "Successfully done. 😮😮" };
            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }

        public async Task<BLStatus> Upsert(List<FestivalBonusSetup> entity)
        {
            try
            {
                //foreach (var item in entity)
                //{
                //    if (item.IsFixed)
                //    {
                //        item.CalcRuleID = -1;
                //    }

                //    if (item.CalcRuleID != -1)
                //    {
                //        item.Amount = await _unitOfWork.Rules.GetValueFromFunctionOfCalculationRule(item.CalcRuleID, item.Rate,
                //                                                   item.EmpId, GlobalFunctions.FirstDayOfTheMonth(item.ApplyDate.Date), GlobalFunctions.LastDayOfTheMonth(item.ApplyDate.Date), item.CompId);
                //    }
                //    else
                //    {
                //        item.Amount = item.FixedAmount;
                //    }

                //}

                await _unitOfWork.Bonus.Upsert(entity);

                var data = JsonConvert.SerializeObject(entity);
                await _mediator.Send(new CreateTransactionLogCommand() { TransectionID = string.Join(",", entity.Select(f => f.BonusId)), CommandType = "Upsert", TransStatement = "Upsert Festival Bonus Set Up", DocumentReferance = data });

                return new BLStatus { Message = "Successfully done. 😮😮" };
            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }

        public async Task<BLStatus> Upsert(FestivalType entity)
        {
            try
            {
                //foreach (var item in entity)
                //{
                //    if (item.IsFixed)
                //    {
                //        item.CalcRuleID = -1;
                //    }

                //    if (item.CalcRuleID != -1)
                //    {
                //        item.Amount = await _unitOfWork.Rules.GetValueFromFunctionOfCalculationRule(item.CalcRuleID, item.Rate,
                //                                                   item.EmpId, GlobalFunctions.FirstDayOfTheMonth(item.ApplyDate.Date), GlobalFunctions.LastDayOfTheMonth(item.ApplyDate.Date), item.CompId);
                //    }
                //    else
                //    {
                //        item.Amount = item.FixedAmount;
                //    }

                //}

                await _unitOfWork.Bonus.Upsert(entity);

                var data = JsonConvert.SerializeObject(entity);
                //await _mediator.Send(new CreateTransactionLogCommand() { TransectionID = string.Join(",", entity.Select(f => f.f)), CommandType = "Upsert", TransStatement = "Upsert Festival Bonus Set Up", DocumentReferance = data });

                return new BLStatus { Message = "Successfully done. 😮😮" };
            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }

        public async Task<BLStatus> Delete(int festBounusId)
        {
            try
            {
                int count = await _unitOfWork.Bonus.DeleteFestivalBonus(festBounusId);
                if (count > 0)
                {
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = festBounusId.ToString(), CommandType = "Delete", TransStatement = "Delete Festival Bonus", DocumentReferance = festBounusId.ToString() });
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

        public async Task<BLStatus> DeleteBonusSetUp(int bounusId)
        {
            try
            {
                int count = await _unitOfWork.Bonus.DeleteFestivalBonusSetUp(bounusId);
                if (count > 0)
                {
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = bounusId.ToString(), CommandType = "Delete", TransStatement = "Delete Festival Bonus", DocumentReferance = bounusId.ToString() });
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
