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
            var antrag = new Foerderantrag();
            
            // Precondition
            Assert.IsNull(antrag.Bewilligung);

            // Act
            var actual = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsAktiv_WithBewilligungAndUnrestrictedFreigabe_ReturnsTrue()
        {
            var freigabe = new Foerdermittelfreigabe {AufrechtBis = null};
            var bewilligung = new Foerderbewilligung {Freigabe = freigabe};
            var antrag = new Foerderantrag {Bewilligung = bewilligung};

            // Act
            var actual = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsTrue(actual);
        }
    }
}
