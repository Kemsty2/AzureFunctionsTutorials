using System;

namespace CrudFunctions.Domain
{
    public class CosmosContainerIdAttribute : Attribute
    {
        public string ContainerId { get; set; }

        public CosmosContainerIdAttribute(string containerId)
        {
            ContainerId = containerId;
        }
    }
}