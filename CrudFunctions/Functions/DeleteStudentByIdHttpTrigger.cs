using CrudFunctions.Domain.Abstractions;
using CrudFunctions.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CrudFunctions.Functions
{
    public class DeleteStudentByIdHttpTrigger
    {
        private readonly ICosmosRepository<Student> _studentRepository;

        public DeleteStudentByIdHttpTrigger(ICosmosRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [OpenApiOperation(operationId: "DeleteStudent", tags: new[] { "Students" }, Summary = "Delete student by his id", Description = "Delete student by his id", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "Id", Visibility = OpenApiVisibilityType.Important)]
        [FunctionName("DeleteStudentByIdHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Students/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await _studentRepository.DeleteItemByIdAsync(id);

            return new OkObjectResult($"Student with id {id} is successfully deleted");
        }
    }
}