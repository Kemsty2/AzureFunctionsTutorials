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
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CrudFunctions.Functions
{
    public class UpdateStudentByIdHttpTrigger
    {
        private readonly ICosmosRepository<Student> _studentRepository;

        public UpdateStudentByIdHttpTrigger(ICosmosRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [OpenApiOperation(operationId: "UpdateStudent", tags: new[] { "Students" }, Summary = "Update a student by id", Description = "Update a student by id", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(Student),
            Description = "JSON request body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Student), Summary = "The response", Description = "This returns the update student")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "Id", Visibility = OpenApiVisibilityType.Important)]
        [FunctionName("UpdateStudentByIdHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Students/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request to add student in cosmos databse.");

            // Get the request data
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Student>(requestBody);

            var item = await _studentRepository.UpdateItemAsync(id, data);

            return new OkObjectResult(item);
        }
    }
}