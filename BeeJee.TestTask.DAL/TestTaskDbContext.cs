using BeeJee.TestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeJee.TestTask.DAL
{
    public class TestTaskDbContext : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }

        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options)
        {
        }
    }
}
