using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Core.Common.DateTimeProvider;
using La.AppServer.DataTransfer.Objects.Dto.Leistungsvergabe;
using Logger.Client;

namespace La.AppServer.Application.Services
{
    public class FoerderantragDto
    {
        public Foerderbewilligung Foerderbewilligung { get; set; }
        public string DataSource { get; set; }
        public string Status { get; set; }
    }

    public class Foerderbewilligung
    {
        //public Foerdermittelfreigabe Foerdermittelfreigabe { get; set; }
        public DateTime? BewilligtVon { get; set; }
        public DateTime? BewilligtBis { get; set; }
    }

    public class Foerdermittelfreigabe
    {
        public DateTime? AufrechtBis { get; set; }
    }

    public class FooBar
    {
        private IEnumerable<FoerderantragDto> GetFoerderantraegeByFilter(IEnumerable<FoerderantragDto> antraege, string filter)
        {
            return filter == "Alle"
                ? antraege.OrderByDescending(AntragIsActive)
                : antraege.Where(AntragIsActive);
        }

        private static bool AntragIsActive(FoerderantragDto antrag)
        {
            return antrag.Aktiv(VitaDateTimeProvider.Instance.Today);
        }
    }

    public static class FoerderantragDtoExtensions
    {
        public static bool Aktiv(this FoerderantragDto foerderantrag, DateTime stichtag)
        {
            // Wenn Fördermittelfreigabe vorhanden, zählt nur diese
            if (foerderantrag.Foerderbewilligung?.Foerdermittelfreigabe != null)
            {
                var fmf = foerderantrag.Foerderbewilligung.Foerdermittelfreigabe;
                return !fmf.AufrechtBis.HasValue || (fmf.AufrechtBis.Value >= stichtag);
            }

            // Wenn keine Fördermittelfreigabe, sondern nur Bewilligung dann zählt nur diese
            if ((foerderantrag.Foerderbewilligung != null) && foerderantrag.Foerderbewilligung.Administriert)
            {
                var fmb = foerderantrag.Foerderbewilligung;
                return (!fmb.BewilligtBis.HasValue || (fmb.BewilligtBis.Value >= stichtag)) &&
                       ((foerderantrag.DataSource != "LgkPlus") || (foerderantrag.Status == "bewilligt"));
            }

            // Ansonsten wird der Antragsstatus verwendet
            //if (foerderantrag.Administriert)
            {
                var filterKeyPerDataSource = "StatusAntragAufrecht_" + foerderantrag.DataSource;
                var statusAntragAufrecht = GetSemicolonSeparatedAppSetting(filterKeyPerDataSource);
                return (statusAntragAufrecht != null) &&
                       statusAntragAufrecht.Contains(foerderantrag.Status ?? string.Empty);
            }

            return false;
        }

        //<add key = "StatusAntragAufrecht_KVS" value=";in Bearb.;in KBV" />
        //<add key = "StatusAntragAufrecht_EdvNeu" value=";bewilligt;Bewilligung;eingelangt;Erfasst;Retour von MA 15" />
        //<add key = "StatusAntragAufrecht_LgkPlus" value="bewilligt;noch keine Entscheidung" />

        private static string[] GetSemicolonSeparatedAppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                Log.Get.DebugFormat(string.Format("ConfigurationManager.AppSettings '{0}' null", key));
                return null;
            }

            return value.Split(';');
        }
    }
}