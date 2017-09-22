using InvestorApi.Contracts;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Newtonsoft.Json;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// A service to provide access to system settings.
    /// </summary>
    public class SettingService : ISettingService
    {
        private const string DefaultAccountSettingsKey = "DEFAULT_ACCOUNT_SETTINGS";
        private const string BuyCommissionsKey = "BUY_COMMISSIONS";
        private const string SellCommissionsKey = "SELL_COMMISSIONS";

        private readonly ISettingRepository _settingRepository;

        private DefaultAccountSettings _defaultAccountSettings;
        private Commissions _buyCommissions;
        private Commissions _sellCommissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingService"/> class.
        /// </summary>
        /// <param name="settingRepository">The setting repository.</param>
        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        /// <summary>
        /// Gets the default account settings.
        /// </summary>
        /// <returns></returns>
        public DefaultAccountSettings GetDefaultAccountSettings()
        {
            if (_defaultAccountSettings == null)
            {
                var setting = _settingRepository.GetByKey(DefaultAccountSettingsKey);
                _defaultAccountSettings = JsonConvert.DeserializeObject<DefaultAccountSettings>(setting.Value);
            }

            return _defaultAccountSettings;
        }

        /// <summary>
        /// Saves the default account settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SaveDefaultAccountSettings(DefaultAccountSettings settings)
        {
            var value = JsonConvert.SerializeObject(settings);
            var setting = Setting.Create(DefaultAccountSettingsKey, value);
            _settingRepository.Save(setting);
        }

        /// <summary>
        /// Gets the buy commissions.
        /// </summary>
        /// <returns></returns>
        public Commissions GetBuyCommissions()
        {
            if (_buyCommissions == null)
            {
                var setting = _settingRepository.GetByKey(BuyCommissionsKey);
                _buyCommissions = JsonConvert.DeserializeObject<Commissions>(setting.Value);
            }

            return _buyCommissions;
        }

        /// <summary>
        /// Saves the buy commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        public void SaveBuyCommissions(Commissions commissions)
        {
            var value = JsonConvert.SerializeObject(commissions);
            var setting = Setting.Create(BuyCommissionsKey, value);
            _settingRepository.Save(setting);
        }

        /// <summary>
        /// Gets the sell commissions.
        /// </summary>
        /// <returns></returns>
        public Commissions GetSellCommissions()
        {
            if (_sellCommissions == null)
            {
                var setting = _settingRepository.GetByKey(SellCommissionsKey);
                _sellCommissions = JsonConvert.DeserializeObject<Commissions>(setting.Value);
            }

            return _sellCommissions;
        }

        /// <summary>
        /// Saves the sell commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        public void SaveSellCommissions(Commissions commissions)
        {
            var value = JsonConvert.SerializeObject(commissions);
            var setting = Setting.Create(SellCommissionsKey, value);
            _settingRepository.Save(setting);
        }
    }
}
