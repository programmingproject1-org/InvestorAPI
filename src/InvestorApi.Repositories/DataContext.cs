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

        public DbSet<Watchlist> Watchlists { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().HasMany(e => e.Accounts);
            modelBuilder.Entity<User>().HasMany(e => e.Watchlists);

            modelBuilder.Entity<Watchlist>().HasKey(e => e.Id);
            modelBuilder.Entity<Watchlist>().Property(e => e.Symbols).HasColumnType("Character_varying");

            modelBuilder.Entity<Account>().HasKey(e => e.Id);
            modelBuilder.Entity<Account>().HasMany(e => e.Positions);
            modelBuilder.Entity<Account>().HasMany(e => e.Transactions);

            modelBuilder.Entity<Position>().HasKey(e => e.Id);

            modelBuilder.Entity<Transaction>().HasKey(e => e.Id);

            modelBuilder.Entity<Setting>().HasKey(e => e.Key);
        }
    }
}
