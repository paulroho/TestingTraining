using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Foerder.Services
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public List<string> GetStatusAntragAufrechtByDataSource(string dataSource)
        {
            var filterKeyPerDataSource = "StatusAntragAufrecht_" + dataSource;
            return GetSemicolonSeparatedAppSetting(filterKeyPerDataSource);
        }

        private List<string> GetSemicolonSeparatedAppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            return value?.Split(';').ToList();
        }
    }
}