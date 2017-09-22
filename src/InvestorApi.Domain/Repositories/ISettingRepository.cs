using InvestorApi.Domain.Entities;

namespace InvestorApi.Domain.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="Setting"/> entities.
    /// </summary>
    public interface ISettingRepository
    {
        /// <summary>
        /// Gets a setting by its unique key.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <returns>The matching setting.</returns>
        Setting GetByKey(string key);

        /// <summary>
        /// Saves the specified setting.
        /// </summary>
        /// <param name="setting">The setting to save.</param>
        void Save(Setting setting);
    }
}
