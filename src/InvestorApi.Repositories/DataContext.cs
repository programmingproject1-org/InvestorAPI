using InvestorApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestorApi.Repositories
{
    public sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<User>().HasMany(e => e.Accounts);
            modelBuilder.Entity<Account>().HasMany(e => e.Positions);
            modelBuilder.Entity<Account>().HasMany(e => e.Transactions);
        }
    }
}
