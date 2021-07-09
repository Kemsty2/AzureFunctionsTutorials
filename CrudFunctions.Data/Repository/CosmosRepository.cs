using CrudFunctions.Domain;
using CrudFunctions.Domain.Abstractions;
using CrudFunctions.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudFunctions.Data.Repository
{
    public class CosmosRepository<TItem> : ICosmosRepository<TItem> where TItem : BaseEntity
    {
        private readonly Container _container;

        public CosmosRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _container = cosmosClient.GetContainer(configuration["CosmosDatabase"], GetContainerId(typeof(TItem)));
        }

        /// <summary>
        /// Add new item in cosmos database container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<TItem> AddItemAsync(TItem item)
        {
            var response = await _container.CreateItemAsync(item, new PartitionKey(item.Id.ToString()));
            return response.Resource;
        }

        /// <summary>
        /// Delete item in cosmos database container by his identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        public virtual async Task DeleteItemByIdAsync(string id)
        {
            await _container.DeleteItemAsync<TItem>(id, new PartitionKey(id));
        }

        /// <summary>
        /// Get item in cosmos database container by his identifier
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        public virtual async Task<TItem> GetItemAsyncById(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<TItem>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all items of a cosmos database container
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ICollection<TItem>> GetAllItems(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = "select * from c";
            }
            var query = _container.GetItemQueryIterator<TItem>(new QueryDefinition(queryString));
            var results = new List<TItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <summary>
        /// Update item in cosmos database container
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="item">new value to update</param>
        /// <returns></returns>
        public virtual async Task<TItem> UpdateItemAsync(string id, TItem item)
        {
            var response = await _container.UpsertItemAsync(item, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Get the containerId of the document based on his CosmosContainerIdAttribute
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        protected string GetContainerId(Type documentType)
        {
            return ((CosmosContainerIdAttribute)documentType.GetCustomAttributes(
                    typeof(CosmosContainerIdAttribute),
                    true)
                .FirstOrDefault())?.ContainerId;
        }
    }
}