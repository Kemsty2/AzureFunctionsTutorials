using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CrudFunctions.Domain.Entities
{
    public class BaseEntity
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}