namespace InvestorApi.Domain.Entities
{
    public class Setting
    {
        private Setting()
        {
        }

        private Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public static Setting Create(string key, string value)
        {
            return new Setting(key, value);
        }
    }
}
