using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvestorApi.Repositories
{
    internal sealed class SettingRepository : ISettingRepository
    {
        private readonly DataContext _context;

        public SettingRepository(DataContext context)
        {
            _context = context;
        }

        public Setting GetByKey(string key)
        {
            return _context.Settings
                .Where(setting => setting.Key == key)
                .FirstOrDefault();
        }

        public void Save(Setting setting)
        {
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
