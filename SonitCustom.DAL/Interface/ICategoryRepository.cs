using SonitCustom.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.DAL.Interface
{
    public interface ICategoryRepository
    {
        Task<int> GetCategoryIdByNameAsync(string cateName);
    }
} 