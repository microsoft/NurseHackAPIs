using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;

namespace HackAPIs.Services.Teams
{
    public class ConfigureFromCommaSeparatedString<TOptions> : IConfigureOptions<TOptions>
            where TOptions : class
    {
        private readonly IConfigurationSection _section;

        public ConfigureFromCommaSeparatedString(IConfigurationSection section)
        {
            _section = section;
        }

        public void Configure(TOptions options)
        {
            var value = _section.Value;
            if (!string.IsNullOrEmpty(value))
            {
                var list = value.Split(',').ToList();
                var property = typeof(TOptions).GetProperty("TeamIds");
                property.SetValue(options, list);
            }
        }
    }
}
