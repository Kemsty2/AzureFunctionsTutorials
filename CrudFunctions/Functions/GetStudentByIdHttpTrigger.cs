using System;
using CrudFunctions.Domain.Abstractions;
using CrudFunctions.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace CrudFunctions.Functions
{
    public class GetStudentByIdHttpTrigger
    {
        private readonly ICosmosRepository<Student> _studentRepository;

        public GetStudentByIdHttpTrigger(ICosmosRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [OpenApiOperation(operationId: "GetStudentById", tags: new[] { "Students" }, Summary = "Gets student by his id", Description = "Gets student by his id", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Student), Summary = "The response", Description = "This returns the response")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "Id", Visibility = OpenApiVisibilityType.Important)]
        [FunctionName("GetStudentByIdHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Students/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var student = await _studentRepository.GetItemAsyncById(id);

            return new OkObjectResult(student);
        }
    }
}