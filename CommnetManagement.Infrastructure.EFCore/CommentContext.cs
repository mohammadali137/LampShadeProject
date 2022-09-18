using CommentManagement.Domain.CommentAgg;
using CommnetManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CommnetManagement.Infrastructure.EFCore
{
    public class CommentContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CommentMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            //modelBuilder.Entity<Comment>()
            //.HasOne(c => c.Parent)
            //.WithMany()
            //.OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);
        }
    }
}
