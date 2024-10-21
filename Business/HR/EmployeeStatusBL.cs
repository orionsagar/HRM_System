using Application.Tasks.Commands.CEmployee;
using Application.Tasks.Commands.CTransactionLog;
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

namespace Business.HR
{
    public interface IEmployeeStatusBL
    {
        //Task<BLStatus> Upsert(Employee emp);
        //Task<BLStatus> Upsert(EmployeeDetails emp);
        //Task<BLStatus> Upsert(EmployeeEducation edu);
        //Task<BLStatus> Upsert(EmployeeExperience exp);
        //Task<BLStatus> Delete(int empId, string userId);

        //Task<string> GenerateCardNo(string compId);
        Task<BLStatus> EmployeeHistroyStatusChange(List<EmployeeStatusVM> entity);
        Task<BLStatus> EmployeeHistroyStatusUpdate(EmployeeStatusVM entity);
        Task<BLStatus> DeleteStatus(List<EmployeeStatusVM> employees);
    }

    public class EmployeeStatusBL : IEmployeeStatusBL
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public EmployeeStatusBL(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<BLStatus> EmployeeHistroyStatusChange(List<EmployeeStatusVM> entity)
        {
            try
            {
                int count = await _mediator.Send(new ChangeEmployeeHistoryStatusCommand { EmployeeStatusVM = entity });
                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", entity.Select(a => a.EmpId)), CommandType = "Create", TransStatement = "Change Employee Staus", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Status Save." };
                }
                else
                {
                    return new BLStatus { Message = "Status not Save!!!" };
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<BLStatus> EmployeeHistroyStatusUpdate(EmployeeStatusVM entity)
        {
            try
            {
                int count = await _mediator.Send(new UpdateEmployeeHistoryStatusCommand { EmployeeStatusVM = entity });
                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(entity);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = entity.EmpHistoryId.ToString(), CommandType = "Update", TransStatement = "Change Employee Staus", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Status Updated." };
                }
                else
                {
                    return new BLStatus { Message = "Status not Updated!!!" };
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<BLStatus> DeleteStatus(List<EmployeeStatusVM> employees)
        {
            try
            {
                // check for default status can not delete initial status
                foreach (var item in employees)
                {
                    var countHistory = _unitOfWork.Employees.CountEmpHistoryByEmpId(item.EmpId);

                    if (countHistory <= 1)
                    {
                        var emp = await _unitOfWork.Employees.GetById(item.EmpId);
                        return new BLStatus { IsError = true, Message = $"Can not delete default status. Name: {emp.Name}, Card No: {emp.CardNo}", Data = countHistory };
                    }
                }


                var deletedCount = await _mediator.Send(new DeleteEmployeeHistoryCommand { EmpHistorysId = employees.Select(a => a.EmpHistoryId).ToList() });

                if (deletedCount > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(employees);
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(",", employees.Select(a => a.EmpId)), CommandType = "Delete", TransStatement = "Delete Employee Staus", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Status deleted.", Data = deletedCount };
                }
                else
                {
                    return new BLStatus { IsError = true, Message = "Something wrong, Please try again.", Data = deletedCount };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
