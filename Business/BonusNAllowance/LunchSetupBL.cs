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
    public interface ILunchSetupBL
    {
        Task<BLStatus> Upsert(SnacksAllowance entity);
        Task<BLStatus> Delete(int advaceId);
        Task<bool> CheckExistByDesignation(SnacksAllowance entity);
    }

    public class LunchSetupBL : ILunchSetupBL
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public LunchSetupBL(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> CheckExistByDesignation(SnacksAllowance entity)
        {
            return await _unitOfWork.LunchSetup.IsExistByDesignation(entity);
        }

        public async Task<BLStatus> Upsert(SnacksAllowance entity)
        {
            try
            {
                if (entity == null)
                {
                    return new BLStatus { Message = "No data found to create/update", StatusCode = "404" };
                }

                int count = 0;
                if (entity.SnacksAllowanceID > 0)
                {

                    count = await _unitOfWork.LunchSetup.Update(entity);
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", entity.SnacksAllowanceID), CommandType = "Update", TransStatement = "Update Lunch Allowance", DocumentReferance = dataJSON });
                }
                else
                {
                    count = await _unitOfWork.LunchSetup.Add(entity);
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", entity.SnacksAllowanceID), CommandType = "Create", TransStatement = "Add Lunch Allowance", DocumentReferance = dataJSON });
                }

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

        public async Task<BLStatus> Delete(int advaceId)
        {
            try
            {
                int count = await _unitOfWork.LunchSetup.Delete(advaceId);
                if (count > 0)
                {
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", advaceId), CommandType = "Create", TransStatement = "Delete Lunch Allowance", DocumentReferance = advaceId.ToString() });
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
