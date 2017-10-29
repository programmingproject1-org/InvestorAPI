using InvestorApi.Domain.Utilities;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a setting.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Setting"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private Setting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="key">The setting key.</param>
        /// <param name="value">The setting value.</param>
        private Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Creates the specified key.
        /// </summary>
        /// <param name="key">The setting key.</param>
        /// <param name="value">The setting value.</param>
        /// <returns>The newly created setting.</returns>
        public static Setting Create(string key, string value)
        {
            Validate.NotNullOrWhitespace(key, nameof(key));
            Validate.NotNullOrWhitespace(value, nameof(value));

            return new Setting(key, value);
        }
    }
}
