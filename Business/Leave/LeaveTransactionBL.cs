using Application.Tasks.Commands.CEarnLeave;
using Application.Tasks.Commands.CLeaveTransaction;
using Application.Tasks.Commands.CShortLeaveSetup;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.Queries.QLeaveType;
using Domains.Models;
using MediatR;
using Newtonsoft.Json;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Leave
{
    public interface ILeaveTransactionBL
    {
        Task<BLStatus> Upsert(LeaveTransaction leaveTransaction, int lTransId, int FiscalYearId, int OrgId);
        Task<BLStatus> LeaveApproved(int LeaveTransactionId, string Status);
        Task<string> CheckBeforeSave(LeaveTransaction lt, int FiscalYearId, int OrgId);
        Task<string> CheckBalance(int EmpId, int FiscalYearId, int LeaveTypeId, int RequestDays, int lTransId, int OrgId);
        Task<BLStatus> DeleteByEmpId(List<int> empIds);
        Task<BLStatus> Delete(int shortLeaveSetupId);
    }
    public class LeaveTransactionBL : ILeaveTransactionBL
    {
        private readonly IMediator _mediatR;
        private readonly IUnitOfWork _unitOfWork;


        public LeaveTransactionBL(IMediator mediatR, IUnitOfWork unitOfWork)
        {
            _mediatR = mediatR;
            _unitOfWork = unitOfWork;
        }        

        public async Task<string> CheckBalance(int EmpId, int FiscalYearId, int LeaveTypeId, int RequestDays,int lTransId, int OrgId)
        {
            var leavedetails = await _unitOfWork.LeaveTransaction.SP_LeaveDetails(EmpId, FiscalYearId, LeaveTypeId,OrgId);
            if (leavedetails != null)
            {
                foreach (var item in leavedetails)
                {                    
                    if (lTransId != 0)
                    {
                        var leavebalance = item.BalanceDays + RequestDays;
                        if (leavebalance >= RequestDays)
                        {
                            return "Ok";
                        }
                        else
                        {
                            return "Total Days Exceeds the Balance";
                        }
                    }
                    else
                    {
                        if (item.BalanceDays >= RequestDays)
                        {
                            return "Ok";
                        }
                        else
                        {
                            return "Total Days Exceeds the Balance";
                        }
                    }                                      
                }
            }
            else
            {
                return "No Leave Setup for the Leave Type";
            }
            return "Ok";
        }

        public async Task<string> CheckBeforeSave(LeaveTransaction lt, int FiscalYearId, int OrgId)
        {
            if (lt.TotalDays <= 0)
            {
                return "Check Total Days";
            }
            var balancecheckmessage = await CheckBalance(lt.EmpId, FiscalYearId, lt.LeaveTypeId, lt.TotalDays,lt.LeaveTransactionId,OrgId);
            if (balancecheckmessage != "Ok")
            {
                return balancecheckmessage;
            }
            else
            {
                //start and end date check in same fiscalyear or not
                var startYearId = _unitOfWork.FiscalYear.GetFiscalYearsByDate(lt.StartDate, lt.CompId);
                if (startYearId == 0)
                {
                    return "No FiscalYear for Start Date";
                }
                var endYearId = _unitOfWork.FiscalYear.GetFiscalYearsByDate(lt.EndDate, lt.CompId);
                if (endYearId == 0)
                {
                    return "No FiscalYear for End Date";
                }
                if (startYearId != endYearId)
                {
                    return "Start Date and End Date Must be in the same fiscal year";
                }
                var leavedetails = await _unitOfWork.LeaveTransaction.GetLeaveTransactions(lt.LeaveTransactionId, lt.EmpId, 0, lt.CompId);
                var IsOverlap = false;
                foreach (var item in leavedetails)
                {
                    if (item.LeaveTransactionId != lt.LeaveTransactionId)
                    {
                        IsOverlap = IsOverlap2DateRange(item.StartDate, item.EndDate, lt.StartDate, lt.EndDate);
                        if (IsOverlap == true)
                        {
                            return "Given Date Range Overlaps with privious Date Range";
                        }
                    }
                }
                // it will add after employeeattendance task done
                //list = new AttendanceBO().getEmployeeAttendanceID(ctlEmployee1.SelectedValue.EmployeeID, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                //if (list.Count != 0)
                //{
                //    MessageBox.Show("There is an Attendance in the Given Date Range");
                //    return false;
                //} 
                var leaveType = await _mediatR.Send(new GetAllLeaveTypeByIdQuery() { LeaveTypeId = lt.LeaveTypeId });
                if ((lt.StartDate - lt.DOJ).TotalDays <= 365 && (leaveType.LTypeName == "Earn Leave"))
                {
                    return "Service Period must be at least 1 year to get Earn Leave";
                }
            }
            return "Ok";
        }

        // Add OR Edit when LeaveType "Earn Leave"
        public async Task<BLStatus> Upsert(LeaveTransaction leaveTransaction, int lTransId, int FiscalYearId,int OrgId)
        {
            try
            {
                string status = "";
                status = await CheckBeforeSave(leaveTransaction, FiscalYearId,OrgId);
                if (status == "Ok")
                {
                    if (leaveTransaction.LeaveTransactionId > 0)
                    {
                        if(leaveTransaction.LeaveTypeName == "Earn Leave")
                        {
                            await _mediatR.Send(new UpdateLeaveTransactionCommand() { LeaveTransaction = leaveTransaction });
                            await _mediatR.Send(new UpdateTotalLeaveTransectionCommand() { FiscalYearId = FiscalYearId, EmpId = leaveTransaction.EmpId, TotalDays = leaveTransaction.TotalDays, LeaveTransactionId = leaveTransaction.LeaveTransactionId });
                            
                        }
                        else
                        {
                            await _mediatR.Send(new UpdateLeaveTransactionCommand() { LeaveTransaction = leaveTransaction });
                        }
                        var dataJSON = JsonConvert.SerializeObject(leaveTransaction);
                        await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = leaveTransaction.LeaveTransactionId.ToString(), CommandType = "Create", TransStatement = "Create leave", DocumentReferance = dataJSON });


                        return new BLStatus { Message = "Leave Update Successfully", };
                    }
                    else
                    {
                        var startYearId = _unitOfWork.FiscalYear.GetFiscalYearsByDate(leaveTransaction.StartDate, leaveTransaction.CompId);
                        if (leaveTransaction.LeaveTypeName == "Earn Leave")
                        {
                            await _mediatR.Send(new CreateLeaveTransactionCommand() { LeaveTransaction = leaveTransaction });
                            await _mediatR.Send(new UpdateTotalLeaveTransectionCommand() { FiscalYearId = FiscalYearId, EmpId = leaveTransaction.EmpId, TotalDays = leaveTransaction.TotalDays, LeaveTransactionId = lTransId });
                        }
                        else
                        {
                            await _mediatR.Send(new CreateLeaveTransactionCommand() { LeaveTransaction = leaveTransaction });
                        }

                        var dataJSON = JsonConvert.SerializeObject(leaveTransaction);
                        await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = leaveTransaction.LeaveTransactionId.ToString(), CommandType = "Update", TransStatement = "Update leave", DocumentReferance = dataJSON });

                        return new BLStatus { Message = "Leave Save Successfully", };
                    }
                }
                else
                {
                    return new BLStatus { Message = status, IsError = true };
                }

            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public bool IsOverlap2DateRange(DateTime PStartDate, DateTime PEndDate, DateTime CStartDate, DateTime CEndDate)
        {
            if ((PEndDate < CEndDate && PEndDate < CStartDate) || (CEndDate < PEndDate && CEndDate < PStartDate))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<BLStatus> DeleteByEmpId(List<int> empIds)
        {
            try
            {
                
                var entity = await _unitOfWork.ShortLeaveSetup.GetByEmpIds(empIds);
                if (empIds.Count < 1)
                {                    
                    return new BLStatus { IsError = true, Message = "Data not found!!", StatusCode = "400" };
                }                

                await _mediatR.Send(new DeleteShortLeaveSetupDataByEmpIdCommand { EmpId = empIds });

                var dataJSON = JsonConvert.SerializeObject(entity);
                await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", empIds), CommandType = "Delete", TransStatement = "Delete Short leave", DocumentReferance = dataJSON });

                return new BLStatus { Message = "Short leave Deleted!!" };
            }
            catch (Exception ex)
            {
                return new BLStatus { Message = ex.ToString() };
            }
        }

        public async Task<BLStatus> Delete(int shortLeaveSetupId)
        {
            try
            {
                var entity = await _unitOfWork.ShortLeaveSetup.GetById(shortLeaveSetupId);
                var data = await _mediatR.Send(new DeleteShortLeaveSetupCommand { ShortLeaveSetupId = shortLeaveSetupId });
                if (data > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediatR.Send(new CreateTransactionLogCommand { TransectionID = shortLeaveSetupId.ToString(), CommandType = "Delete", TransStatement = "Delete Short Leave Setup Data", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Short leave Deleted!!" };
                }
                else
                {
                    return new BLStatus { IsError = true, Message = "Short leave not Deleted!!" };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BLStatus> LeaveApproved(int LeaveTransactionId, string Status)
        {
            await _unitOfWork.LeaveTransaction.UpdateLeaveApprovalStatus(LeaveTransactionId, Status);
            return new BLStatus { IsError = false,Data = $"{Status}", Message = "Leave Approved", StatusCode= "200" };
        }
    }
}
