using System.Collections.Generic;

namespace Foerder.Services
{
    public interface IConfigurationProvider
    {
        List<string> GetStatusAntragAufrechtByDataSource(string dataSource);
    }
}