using System;
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
            return false;
        }
    }
}