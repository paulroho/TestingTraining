using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Foerder.Domain;

namespace Foerder.Services
{
    public class FoerderantragService
    {
        public bool IsAktiv(Foerderantrag antrag, DateTime stichtag)
        {
            if (antrag.Bewilligung?.Freigabe != null)
            {
                var freigabe = antrag.Bewilligung.Freigabe;
                return !freigabe.AufrechtBis.HasValue || (freigabe.AufrechtBis.Value >= stichtag);
            }

            var statusAntragAufrecht = GetStatusAntragAufrechtByDataSource(antrag.DataSource);
            return (statusAntragAufrecht != null) &&
                    statusAntragAufrecht.Contains(antrag.Status ?? string.Empty);
        }

        private List<string> GetStatusAntragAufrechtByDataSource(string dataSource)
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