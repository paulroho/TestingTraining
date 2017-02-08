using System;
using Foerder.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foerder.Services.Tests
{
    [TestClass]
    public class FoerderantragServiceIsAktivTests
    {
        private FoerderantragService _service;
        private DateTime AnyStichtag { get; } = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            _service = new FoerderantragService();
        }

        [TestMethod]
        public void WithoutBewilligung_ShouldBeInaktiv()
        {
            var antrag = GetAntragWithoutBewilligung();

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsFalse(isAktiv);
        }

        [TestMethod]
        public void WithBewilligungAndUnrestrictedFreigabe_ShouldBeAktiv()
        {
            var antrag = GetAntragWithUnrestrictedFreigabe();

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtADateBeforeTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = GetAntragWithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag:aufrechtBis.AddDays(-1));

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = GetAntragWithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis);

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtADateAfterTheFreigabeValidityDate_ShouldBeInaktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = GetAntragWithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis.AddDays(+1));

            Assert.IsFalse(isAktiv);
        }

        private static Foerderantrag GetAntragWithFreigabeUntil(DateTime aufrechtBis) => GetAntragWithFreigabe(aufrechtBis);

        private static Foerderantrag GetAntragWithUnrestrictedFreigabe() => GetAntragWithFreigabe(null);

        private static Foerderantrag GetAntragWithFreigabe(DateTime? stichtag)
        {
            var freigabe = new Foerdermittelfreigabe {AufrechtBis = stichtag};
            var bewilligung = new Foerderbewilligung {Freigabe = freigabe};
            return new Foerderantrag {Bewilligung = bewilligung};
        }

        private static Foerderantrag GetAntragWithoutBewilligung()
        {
            var antrag = new Foerderantrag();

            if (antrag.Bewilligung != null)
                throw new Exception("The Antrag must not have a Bewilligung.");

            return antrag;
        }
    }
}
