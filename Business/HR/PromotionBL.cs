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

namespace Business.HR
{
    public interface IPromotionBL
    {
        Task<BLStatus> Add(List<PromotionVM> model);
        //Task<int> Delete(int id);
        //Task<IEnumerable<PromotionVM>> GetAll();
        //Task<Department> GetById(int id);
        //Task<int> Update(Promotion entity);
        //Task<int> Add(PromotionVM psvm);
        //Task<double> SalaryCalculation(PromotionVM psvm);
    }
    public class PromotionBL : IPromotionBL
    {
        private IUnitOfWork _unitOfWork;
        private IMediator _mediator;
        private IMapper _mapper;

        public PromotionBL(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<BLStatus> Add(List<PromotionVM> model)
        {
            List<EmployeeHistory> histories = new List<EmployeeHistory>();
            foreach (var item in model)
            {
                var empHistory = await _unitOfWork.Employees.GetEmpHistoryById(item.EmpHistoryId);
                if (empHistory != null)
                {
                    empHistory.EmpHistoryId = 0;
                    empHistory.DesigId = item.DesignationId;
                    empHistory.PayScaleId = item.PayScaleId;
                    empHistory.Salary = item.Salary;
                    empHistory.EffectiveDate = item.EffectiveDate;
                    empHistory.DecisionDate = item.DecisionDate;
                    empHistory.TransType = (int)Enums.TransactionType.Promotion;
                    empHistory.Remarks = "Promotion: "+item.Remarks;
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
                await _mediator.Send(new CreateTransactionLogCommand { TransectionID = String.Join(", ", model.Select(a => a.EmpId)), CommandType = "Create", TransStatement = "Employee Promotion", DocumentReferance = dataJSON });

                return new BLStatus { Message = "Successfully Saved." };
            }
            else
            {
                return new BLStatus {IsError=true, Message = "No data for save." };
            }
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PromotionVM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Department> GetById(int id)
        {
            throw new NotImplementedException();
        }


    }
}
