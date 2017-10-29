using InvestorApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace InvestorApi.Repositories
{
    /// <summary>
    /// The Entity framework database context to access the application's Postgres database.
    /// </summary>
    public sealed class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the watchlists.
        /// </summary>
        public DbSet<Watchlist> Watchlists { get; set; }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the positions.
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public DbSet<Setting> Settings { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

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
