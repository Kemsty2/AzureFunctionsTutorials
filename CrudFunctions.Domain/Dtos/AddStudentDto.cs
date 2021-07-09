using CrudFunctions.Domain.Entities;
using System;

namespace CrudFunctions.Domain.Dtos
{
    public class AddStudentDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}