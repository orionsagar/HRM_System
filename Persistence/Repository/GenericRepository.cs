using Domains.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
   
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<int> Add(T entity);
        Task<int> Delete(int id);
        Task<int> Update(T entity);
        
    }

    public interface IGenericRepositoryNew<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll(int OrgId);
        Task<int> Add(T entity);
        Task<int> Delete(int id);
        Task<int> Update(T entity);              
    }
}
