using CrudFunctions.Domain.Abstractions;
using CrudFunctions.Domain.Dtos;
using CrudFunctions.Domain.Entities;
using CrudFunctions.Functions;
using CrudFunctions.Tests.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CrudFunctions.Tests
{
    public class AddStudentFunctionTest
    {
        [Fact]
        public async Task ShouldCreateStudent()
        {
            //  Arrange
            var json = new AddStudentDto()
            {
                Name = "test",
                DateOfBirth = DateTime.Now,
                Email = "test@test.com",
                Gender = GenderEnum.Male,
                LastName = "test"
            };

            var expectedStudent = new Student(json);

            var mockStudentRepo = new Mock<ICosmosRepository<Student>>();
            mockStudentRepo.Setup(x => x.AddItemAsync(It.IsAny<Student>())).ReturnsAsync(expectedStudent);

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>(),
                JsonConvert.SerializeObject(json));

            //  Act
            var function = new AddStudentFunctionHttpTrigger(mockStudentRepo.Object);
            var response = (CreatedResult)await function.Run(request, TestFactory.CreateLogger());

            //  Assert
            Assert.Equal(expectedStudent, response.Value);
        }

        [Fact]
        public async Task ShouldNotCreateStudent()
        {
            //  Arrange
            var json = new AddStudentDto()
            {
                Name = "test",
                DateOfBirth = DateTime.Now,
                Email = "test@test.com",
                Gender = GenderEnum.Male,
                LastName = "test"
            };

            var mockStudentRepo = new Mock<ICosmosRepository<Student>>();
            mockStudentRepo.Setup(x => x.AddItemAsync(It.IsAny<Student>())).Throws(new Exception("Une erreur est survenue"));

            var request = TestFactory.CreateHttpRequest(new Dictionary<string, StringValues>(),
                JsonConvert.SerializeObject(json));

            //  Act
            var function = new AddStudentFunctionHttpTrigger(mockStudentRepo.Object);
            Task Act() => function.Run(request, TestFactory.CreateLogger());

            //  Assert
            var exception = await Assert.ThrowsAsync<Exception>(Act);
            Assert.Equal("Une erreur est survenue", exception.Message);
        }
    }
}