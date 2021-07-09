using CrudFunctions.Domain.Abstractions;
using CrudFunctions.Domain.Dtos;
using CrudFunctions.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CrudFunctions.Functions
{
    public class AddStudentFunctionHttpTrigger
    {
        private readonly ICosmosRepository<Student> _studentRepository;

        public AddStudentFunctionHttpTrigger(ICosmosRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [OpenApiOperation(operationId: "AddStudent", tags: new[] { "Students" }, Description = "Add new student", Summary = "Add new studen")]
        [OpenApiRequestBody("application/json", typeof(AddStudentDto),
            Description = "JSON request body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Student),
            Description = "Information About the student created")]
        [FunctionName("AddStudentFunctionHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Students")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request to add student in cosmos databse.");

            // Get the request data
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddStudentDto>(requestBody);

            var item = await _studentRepository.AddItemAsync(CreateStudent(data));

            return new CreatedResult("StudentGetById", item);
        }

        private static Student CreateStudent(AddStudentDto dto)
        {
            return new Student(dto);
        }
    }
}