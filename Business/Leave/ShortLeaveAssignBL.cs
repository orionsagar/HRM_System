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

namespace Business.Leave
{
    public interface IShortLeaveAssignBL
    {
        Task<BLStatus> Upsert(List<ShortLeaveAssign> entity);
        Task<BLStatus> Delete(int advaceId);
    }

    public class ShortLeaveAssignBL : IShortLeaveAssignBL
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ShortLeaveAssignBL(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<BLStatus> Upsert(List<ShortLeaveAssign> entity)
        {
            try
            {
                if (entity.Count < 1)
                {
                    return new BLStatus { Message = "No data found to create/update", StatusCode = "404" };
                }

                List<ShortLeaveAssign> duplicateDateData=new List<ShortLeaveAssign>();
                foreach (var item in entity)
                {
                    bool checkDuplicateOnDate = await _unitOfWork.ShortLeaveAssign.CheckDuplicateSameDate(item);
                    if (checkDuplicateOnDate)
                    {
                        var emp=await _unitOfWork.Employees.GetById(item.EmpId);
                        return new BLStatus { IsError=true, Message = $"Already assign a short leave on the date. Emp. Name: {emp.Name} Card No: {emp.CardNo}", StatusCode = "404" };
                    }
                }
                                
                int count = await _unitOfWork.ShortLeaveAssign.Upsert(entity);

                var dataJSON = JsonConvert.SerializeObject(entity);
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", entity.Select(a => a.ShortLeaveAssignId)), CommandType = "Upsert", TransStatement = "Upsert Short Leave Assign", DocumentReferance = dataJSON });

                if (count > 0)
                {
                    return new BLStatus { Message = "Successfully done. 😮😮" };
                }
                else
                    return new BLStatus { Message = "No data found to create/update", StatusCode = "404" };

            }
            catch (Exception ex)
            {
                return new BLStatus { IsError = true, StatusCode = "500", Message = "Something wrong 😥😥" };
            }

        }

        public async Task<BLStatus> Delete(int shortLeaveAssignId)
        {
            try
            {
                var entity = await _unitOfWork.ShortLeaveAssign.GetById(shortLeaveAssignId);
                int count = await _unitOfWork.ShortLeaveAssign.Delete(shortLeaveAssignId);
                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = shortLeaveAssignId.ToString(), CommandType = "Delete", TransStatement = "Delete Short Leave Assign", DocumentReferance = dataJSON });

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
