using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api
{
    // Create ToDoDbContext with Entity Framework
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }
    }
}