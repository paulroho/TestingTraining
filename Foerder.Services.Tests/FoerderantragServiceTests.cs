using System;
using Foerder.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foerder.Services.Tests
{
    [TestClass]
    public class FoerderantragServiceTests
    {
        private FoerderantragService _service;
        private DateTime AnyStichtag { get; } = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            _service = new FoerderantragService();
        }

        [TestMethod]
        public void IsAktiv_WithoutBewilligung_ReturnsFalse()
        {
            var antrag = GetAntragWithoutBewilligung();

            // Act
            var actual = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsAktiv_WithBewilligungAndUnrestrictedFreigabe_ReturnsTrue()
        {
            var antrag = GetAntragWithUnrestrictedFreigabe();

            // Act
            var actual = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsTrue(actual);
        }

        private static Foerderantrag GetAntragWithUnrestrictedFreigabe()
        {
            var freigabe = new Foerdermittelfreigabe {AufrechtBis = null};
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
