using Application.DomainEventFramework.Core;
using Application.Tasks.Commands.CTransactionLog;
using Application.Tasks.DomainEvents;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Newtonsoft.Json;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Application.Tasks.Commands.CEmployee
{
    public class CreateEmployeeCommand : IRequest<BLStatus>
    {
        public Employee Employee { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, BLStatus>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactable _transaction;
        private readonly IDomainEventPublish _publisher;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, ITransactable transaction, IDomainEventPublish publisher)
        {
            _unitOfWork = unitOfWork;
            _transaction = transaction;
            _publisher = publisher;
        }
        public async Task<BLStatus> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cardno = await _unitOfWork.Employees.GenerateCardNo(request.Employee.CompId.ToString(), (int)request.Employee.OrgId);
                using var trans = await _transaction.BeginNewTransationAsync();

                if (request.Employee.EmpId < 1)  // create
                {
                    request.Employee.CardNo = cardno;
                    request.Employee.EmployeeHistory.Remarks = "Joining";
                    //emp.EmployeeHistory.TransType =(int) Enums.TransactionType.Joining; added in view
                    var inserted = await _unitOfWork.Employees.Upsert(request.Employee);

                    // ============= Send Email start =============== //
                    var domainevent = new DomainEventVM
                    {
                        MailSubject = "Employee Registration",
                        MailFor = Enums.MailFor.Employee.ToString(),
                        Email = request.Employee.Email,
                        Phone = request.Employee.Phone,
                        CountryCode = request.Employee.CountryCode,
                        SupportPhone = GlobalFunctions.SupportPhone,
                        SupportMail = GlobalFunctions.SupportMail
                    };
                    var message = new EmployeeEvents(domainevent);

                    if (domainevent.Email != null)
                    {
                        _publisher.Publish(EventNames.Greetings, message, request.Employee.AddedBy.ToString());
                    }
                    // ============= Send Email end =============== //

                    var dataJSON = JsonConvert.SerializeObject(request.Employee, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _transaction.FinishTransactionAsync();
                    return new BLStatus { Message = "Employee created!", Data = new { inserted.EmpId, inserted.EmpHistoryId } };
                }
                else // update
                {
                    request.Employee.EmployeeHistory.Remarks = "Modified from Employee Module";
                    var updated = await _unitOfWork.Employees.Upsert(request.Employee);

                    var dataJSON = JsonConvert.SerializeObject(request.Employee, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    await _transaction.FinishTransactionAsync();
                    return new BLStatus { Message = "Employee updated!", Data = new { updated.EmpId, updated.EmpHistoryId } };
                }                
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
