using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApi.Models
{
    public class ToDoListContext : IdentityDbContext<ApplicationUser>
    {
        public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options) { }

        public DbSet<TaskToDo> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
