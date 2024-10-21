using Application.DomainEventFramework.Core;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.DomainEvents;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
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
    public interface IEmployeeBL
    {
        Task<BLStatus> Upsert(Employee emp);
        Task<BLStatus> Upsert(EmployeeDetails emp);
        Task<BLStatus> Upsert(EmployeeEducation edu);
        Task<BLStatus> Upsert(EmployeeExperience exp);
        Task<BLStatus> Upsert(EmployeeTraining training);
        Task<BLStatus> Upsert(PayDetails payDetails);
        Task<BLStatus> Delete(int empId, string userId);
        Task<string> GenerateCardNo(string compId, int OrgId);

    }

    public class EmployeeBL : IEmployeeBL
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IDomainEventPublish publisher;

        public EmployeeBL(IUnitOfWork unitOfWork, IMediator mediator, IDomainEventPublish publisher)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            this.publisher = publisher;
        }

        public async Task<string> GenerateCardNo(string compId, int OrgId)
        {
            return await _unitOfWork.Employees.GenerateCardNo(compId, OrgId);
        }

        public async Task<BLStatus> Upsert(Employee emp)
        {
            try
            {
                //emp.EmployeeHistory = new EmployeeHistory
                //{
                //    EmpId = emp.EmpId
                //};
                //emp.EmpHistoryId = emp.EmployeeHistory.EmpHistoryId;

                //emp.EmployeeHistory.EmpStatusID = 1; // for active employee
                if (await _unitOfWork.Employees.CheckDuplicateFingerId(fingerId:emp.ProximityNo, empId:emp.EmpId, compId:emp.CompId))
                {
                    return new BLStatus { Message = "Finger Print Id or Proximity no already exist.", IsError=true };
                }
                if (emp.EmpId < 1)  // create
                {
                    emp.CardNo =await _unitOfWork.Employees.GenerateCardNo(emp.CompId.ToString(),(int)emp.OrgId);
                    emp.EmployeeHistory.Remarks = "Joining";
                    //emp.EmployeeHistory.TransType =(int) Enums.TransactionType.Joining; added in view
                    var inserted = await _unitOfWork.Employees.Upsert(emp);

                    // ============= Send Email start =============== //
                    var domainevent = new DomainEventVM
                    {
                       
                        MailSubject = "Employee Registration",
                        MailFor = Enums.MailFor.Employee.ToString(),
                        Email = emp.Email,
                        Phone = emp.Phone,
                        CountryCode = emp.CountryCode,
                        SupportPhone = GlobalFunctions.SupportPhone,
                        SupportMail = GlobalFunctions.SupportMail
                    };
                    var message = new EmployeeEvents(domainevent);

                    if (domainevent.Email != null)
                    {
                        publisher.Publish(EventNames.InviteEmployee, message, emp.AddedBy.ToString());
                    }
                    // ============= Send Email end =============== //

                    var dataJSON = JsonConvert.SerializeObject(emp, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = inserted.EmpId.ToString(), CommandType = "Create", TransStatement = "Create Employee", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee created!", Data = new { inserted.EmpId, inserted.EmpHistoryId } };
                }
                else // update
                {
                    emp.EmployeeHistory.Remarks = "Modified from Employee Module";
                    var updated = await _unitOfWork.Employees.Upsert(emp);

                    var dataJSON = JsonConvert.SerializeObject(emp, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = updated.EmpId.ToString(), CommandType = "Update", TransStatement = "Update Employee", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee updated!", Data = new { updated.EmpId, updated.EmpHistoryId } };
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<BLStatus> Upsert(EmployeeDetails emp)
        {
            try
            {
                if (emp.EmpId < 1)
                {
                    return new BLStatus { Message = "Employee not found!!" };
                }

                if (emp.EmpDetailsID < 1)  // create
                {
                    int id = await _unitOfWork.Employees.Upsert(emp);

                    var dataJSON = JsonConvert.SerializeObject(emp, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = "Create", TransStatement = "Create Employee Details", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee details created!", Data = id };
                }
                else // update
                {
                    int id = await _unitOfWork.Employees.Upsert(emp);

                    var dataJSON = JsonConvert.SerializeObject(emp, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID=id.ToString(), CommandType = "Update", TransStatement = "Update Employee Details", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee details updated!", Data = id };
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<BLStatus> Upsert(EmployeeEducation edu)
        {
            try
            {
                if (edu.EmpId < 1)
                {
                    return new BLStatus { Message = "Employee not found!!" };
                }

                int id = await _unitOfWork.Employees.Upsert(edu);

                if (id > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(edu, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = "Create", TransStatement = "Create Employee Education", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee education updated!", Data = id };
                }
                else
                {
                    var dataJSON = JsonConvert.SerializeObject(edu, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = "Update", TransStatement = "Update Employee Education", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee education created!", Data = id };
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<BLStatus> Upsert(EmployeeExperience expr)
        {
            try
            {
                if (expr.EmpId < 1)
                {
                    return new BLStatus { Message = "Employee not found!!" };
                }

                int count = await _unitOfWork.Employees.Upsert(expr);

                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(expr, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { CommandType = "Create", TransStatement = "Create Employee Experience", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee experience updated!", Data = count };
                }
                else
                {
                    var dataJSON = JsonConvert.SerializeObject(expr, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand {  CommandType = "Update", TransStatement = "Update Employee Experience", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee experience created!", Data = count };
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<BLStatus> Delete(int empId, string userId)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetById(empId);

                if (employee != null)
                {
                    employee.IsDelete = true;
                    employee.UpdatedDate = DateTime.Now;
                    employee.UpdatedBy = userId;
                    var update = await _unitOfWork.Employees.Upsert(employee);

                    var dataJSON = JsonConvert.SerializeObject(employee, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { TransectionID=empId.ToString(), CommandType = "Delete", TransStatement = "Delete Employee", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee deleted!", Data = update.EmpId };
                }
                else
                {
                    throw new KeyNotFoundException($"Employee data not found.");
                }

            }
            catch (Exception ex)
            {
                throw  ex;
            }

        }

        public async Task<BLStatus> Upsert(EmployeeTraining training)
        {
            try
            {
                //if (training.EmpId < 1)
                //{
                //    return new BLStatus { Message = "Employee not found!!" };
                //}

                int count = await _unitOfWork.Employees.Upsert(training);

                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(training, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { CommandType = "Create", TransStatement = "Create Employee Training", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee training update!", Data = count };
                }
                else
                {
                    var dataJSON = JsonConvert.SerializeObject(training, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { CommandType = "Update", TransStatement = "Update Employee Training", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee training entry!", Data = count };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BLStatus> Upsert(PayDetails payDetails)
        {
            try
            {
                int count = await _unitOfWork.Employees.Upsert(payDetails);

                if (count > 0)
                {
                    var dataJSON = JsonConvert.SerializeObject(payDetails, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { CommandType = "Create", TransStatement = "Create Employee Training", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee Payment Details update!", Data = count };
                }
                else
                {
                    var dataJSON = JsonConvert.SerializeObject(payDetails, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    await _mediator.Send(new CreateTransactionLogCommand { CommandType = "Update", TransStatement = "Update Employee Training", DocumentReferance = dataJSON });

                    return new BLStatus { Message = "Employee Payment Details entry!", Data = count };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
