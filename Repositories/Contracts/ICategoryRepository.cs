using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ICategoryRepository:IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        Task<Category> GetACategoryByIdAsync(int id,bool trackChanges);
        void CreateACategory(Category category);
        void UpdateACategory(Category category);
        void DeleteACategory(Category category);
    }
}
