using Newtonsoft.Json;
using System;
using CrudFunctions.Domain.Dtos;

namespace CrudFunctions.Domain.Entities
{
    [CosmosContainerId("students")]
    public class Student : BaseEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public GenderEnum Gender { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        public Student()
        {
        }

        public Student(AddStudentDto dto) : base()
        {
            Name = dto.Name;
            LastName = dto.LastName;
            Gender = dto.Gender;
            Email = dto.Email;
            DateOfBirth = dto.DateOfBirth;
        }
    }

    public enum GenderEnum
    {
        Male,
        Female,
        Others
    }
}