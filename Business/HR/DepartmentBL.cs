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

namespace Business.HR
{
    public interface IDepartmentBL
    {
        Task<int> Add(Department entity);
        Task<int> Delete(int id);
        Task<IEnumerable<Department>> GetAll();
        Task<Department> GetById(int id);
        Task<int> Update(Department entity);
    }

    public class DepartmentBL : GenericBL<Department>, IDepartmentBL
    {
        private IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;

        public DepartmentBL(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Add(Department entity)
        {
            var result = await unitOfWork.Departments.Add(entity);

            var json = JsonConvert.SerializeObject(entity);
            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = entity.DeptId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Create), TransStatement = "Add Department", DocumentReferance = json });

            return result;
        }

        public async Task<int> Delete(int id)
        {
            var result = await unitOfWork.Departments.Delete(id);

            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = id.ToString(), CommandType = Enum.GetName(Enums.commandtype.Delete), TransStatement = "Delete Department", DocumentReferance = id.ToString() });

            return result;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await unitOfWork.Departments.GetAll();
        }

        public async Task<Department> GetById(int id)
        {
            return await unitOfWork.Departments.GetById(id);
        }

        public async Task<int> Update(Department entity)
        {
            var result = await unitOfWork.Departments.Update(entity);

            var json = JsonConvert.SerializeObject(entity);
            await _mediator.Send(new CreateTransactionLogCommand { TransectionID = entity.DeptId.ToString(), CommandType = Enum.GetName(Enums.commandtype.Update), TransStatement = "Update Department", DocumentReferance = json });

            return result;
        }
    }
}
