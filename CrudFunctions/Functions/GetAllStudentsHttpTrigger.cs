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
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CrudFunctions.Functions
{
    public class GetAllStudentsHttpTrigger
    {
        private readonly ICosmosRepository<Student> _studentRepository;

        public GetAllStudentsHttpTrigger(ICosmosRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [OpenApiOperation(operationId: "GetAllStudent", tags: new[] { "Students" }, Summary = "Gets all the students", Description = "Gets all the students", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ICollection<Student>), Summary = "The response", Description = "This returns the response")]
        [FunctionName("GetAllStudentsHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Students")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request. Get All Students");

            var students = await _studentRepository.GetAllItems();

            return new OkObjectResult(students);
        }
    }
}