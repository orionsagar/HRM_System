using Application.Tasks.Commands.CEarnLeave;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QEarnLeave;
using Domains.Models;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Leave
{
    public interface IEarnLeaveBL
    {
        Task<BLStatus> Upsert(EarnLeave earnLeave);
    }
    public class EarnLeaveBL : IEarnLeaveBL
    {
        private readonly IMediator _mediatR;


        public EarnLeaveBL(IMediator mediatR)
        {
            _mediatR = mediatR;
        }
        public async Task<BLStatus> Upsert(EarnLeave earnLeave)
        {
            var earnData = await _mediatR.Send(new GetLeaveByYearIdAndEmpIdQuery() { FiscalYearId = earnLeave.FiscalYearId, EmpId = earnLeave.EmpId });
            if(earnLeave.EarnLeaveId == 0)
            {
                if(earnData == null)
                {
                    await _mediatR.Send(new CreateEarnLeaveCommand() { earnLeave = earnLeave });
                    var dataJSON = JsonConvert.SerializeObject(earnLeave);
                    await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = earnLeave.EarnLeaveId.ToString(), CommandType = "Create", TransStatement = "Create Earn leave", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Earn Leave Created!!" };
                }
                else
                {
                    return new BLStatus { Message = "Earn Leave Already Open!!", IsError = true };
                }                
            }
            else
            {
                earnData.OpeningBalance = earnData.OpeningBalance + earnLeave.OpeningBalance;
                earnData.TotalEarnLeave = earnData.OpeningBalance;
                earnData.UpdatedBy = earnLeave.AddedBy;
                earnData.UpdatedDate = earnLeave.AddedDate;
                await _mediatR.Send(new UpdateEarnLeaveCommand() { earnLeave = earnData });
                var dataJSON = JsonConvert.SerializeObject(earnLeave);
                await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = earnLeave.EarnLeaveId.ToString(), CommandType = "Update", TransStatement = "Update Earn leave", DocumentReferance = dataJSON });

                return new BLStatus { Message = "Earn Leave updated!!" };
            }            
            
        }
    }
}
