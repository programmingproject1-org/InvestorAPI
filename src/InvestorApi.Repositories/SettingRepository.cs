using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InvestorApi.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="Setting"/> entities.
    /// </summary>
    internal sealed class SettingRepository : ISettingRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingRepository"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        public SettingRepository(DataContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        /// <summary>
        /// Gets a setting by its unique key.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <returns>The matching setting.</returns>
        public Setting GetByKey(string key)
        {
            return _context.Settings
                .Where(setting => setting.Key == key)
                .FirstOrDefault();
        }

        /// <summary>
        /// Saves the specified setting.
        /// </summary>
        /// <param name="setting">The setting to save.</param>
        public void Save(Setting setting)
        {
            // Check if the item exists and then either create or update it in the database.
            var exists = _context.Settings.AsNoTracking().Any(x => x.Key == setting.Key);
            if (exists)
            {
                _context.Settings.Update(setting);
            }
            else
            {
                _context.Settings.Add(setting);
            }

            _context.SaveChanges();
        }
    }
}
