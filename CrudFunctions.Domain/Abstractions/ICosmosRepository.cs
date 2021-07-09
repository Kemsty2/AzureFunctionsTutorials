using CrudFunctions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudFunctions.Domain.Abstractions
{
    public interface ICosmosRepository<TItem> where TItem : BaseEntity
    {
        /// <summary>
        /// Add new item in cosmos database container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<TItem> AddItemAsync(TItem item);

        /// <summary>
        /// Delete item in cosmos database container by his identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        Task DeleteItemByIdAsync(string id);

        /// <summary>
        /// Get item in cosmos database container by his identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        Task<TItem> GetItemAsyncById(string id);

        /// <summary>
        /// Get all items of a cosmos database container
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TItem>> GetAllItems(string queryString = "");

        /// <summary>
        /// Update item in cosmos database container
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="item">new value to update</param>
        /// <returns></returns>
        Task<TItem> UpdateItemAsync(string id, TItem item);
    }
}