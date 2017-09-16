using InvestorApi.Domain.Entities;

namespace InvestorApi.Domain.Repositories
{
    public interface ISettingRepository
    {
        Setting GetByKey(string key);

        void Save(Setting setting);
    }
}
