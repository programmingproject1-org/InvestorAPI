using InvestorApi.Contracts;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace InvestorApi.Domain.Services
{
    public class SettingService : ISettingService
    {
        private const string DefaultAccountSettingsKey = "DEFAULT_ACCOUNT_SETTINGS";
        private const string BuyCommissionsKey = "BUY_COMMISSIONS";
        private const string SellCommissionsKey = "SELL_COMMISSIONS";

        private readonly ISettingRepository _settingRepository;

        private DefaultAccountSettings _defaultAccountSettings;
        private Commissions _buyCommissions;
        private Commissions _sellCommissions;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public DefaultAccountSettings GetDefaultAccountSettings()
        {
            if (_defaultAccountSettings == null)
            {
                var setting = _settingRepository.GetByKey(DefaultAccountSettingsKey);
                _defaultAccountSettings = JsonConvert.DeserializeObject<DefaultAccountSettings>(setting.Value);
            }

            return _defaultAccountSettings;
        }

        public void SaveDefaultAccountSettings(DefaultAccountSettings settings)
        {
            var value = JsonConvert.SerializeObject(settings);
            var setting = Setting.Create(DefaultAccountSettingsKey, value);
            _settingRepository.Save(setting);
        }

        public Commissions GetBuyCommissions()
        {
            if (_buyCommissions == null)
            {
                var setting = _settingRepository.GetByKey(BuyCommissionsKey);
                _buyCommissions = JsonConvert.DeserializeObject<Commissions>(setting.Value);
            }

            return _buyCommissions;
        }

        public void SaveBuyCommissions(Commissions commissions)
        {
            var value = JsonConvert.SerializeObject(commissions);
            var setting = Setting.Create(BuyCommissionsKey, value);
            _settingRepository.Save(setting);
        }

        public Commissions GetSellCommissions()
        {
            if (_sellCommissions == null)
            {
                var setting = _settingRepository.GetByKey(SellCommissionsKey);
                _sellCommissions = JsonConvert.DeserializeObject<Commissions>(setting.Value);
            }

            return _sellCommissions;
        }

        public void SaveSellCommissions(Commissions commissions)
        {
            var value = JsonConvert.SerializeObject(commissions);
            var setting = Setting.Create(SellCommissionsKey, value);
            _settingRepository.Save(setting);
        }
    }
}
