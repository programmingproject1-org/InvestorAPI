using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace InvestorApi.Converters
{
    /// <summary>
    /// Extends the default <see cref="IsoDateTimeConverter"/> to format dates without time correctly.
    /// </summary>
    public class OnlyDateConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0)
                {
                    writer.WriteValue(dateTime.ToString("yyyy-MM-dd"));
                    return;
                }
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}
