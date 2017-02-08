using System;
using Foerder.Domain;

namespace Foerder.Services.Tests.TestDataBuilders
{
    public class AntragBuilder
    {
        public Foerderantrag WithoutBewilligung()
        {
            var antrag = new Foerderantrag();

            if (antrag.Bewilligung != null)
                throw new Exception("The Antrag must not have a Bewilligung.");

            return antrag;
        }

        public Foerderantrag WithUnrestrictedFreigabe() => GetAntragWithFreigabe(null);

        public Foerderantrag WithFreigabeUntil(DateTime aufrechtBis) => GetAntragWithFreigabe(aufrechtBis);

        private static Foerderantrag GetAntragWithFreigabe(DateTime? stichtag)
        {
            var freigabe = new Foerdermittelfreigabe { AufrechtBis = stichtag };
            var bewilligung = new Foerderbewilligung { Freigabe = freigabe };
            return new Foerderantrag { Bewilligung = bewilligung };
        }
    }
}