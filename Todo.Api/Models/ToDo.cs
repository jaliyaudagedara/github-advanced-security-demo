using System;

namespace Todo.Api.Models
{
    // Create ToDo class
    public class ToDo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? CompletedDateTime { get; set; }
    }
}