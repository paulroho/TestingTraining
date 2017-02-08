using System;
using Foerder.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Foerder.Services.Tests
{
    [TestClass]
    public class FoerderantragServiceTests
    {
        private DateTime AnyStichtag { get; } = DateTime.Now;

        [TestMethod]
        public void IsAktiv_WithoutBewilligung_ReturnsFalse()
        {
            var antrag = new Foerderantrag();
            var service = new FoerderantragService();
            
            // Precondition
            Assert.IsNull(antrag.Bewilligung);

            // Act
            var actual = service.IsAktiv(antrag, AnyStichtag);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsAktiv_WithBewilligungAndUnrestrictedFreigabe_ReturnsTrue()
        {
            var freigabe = new Foerdermittelfreigabe {AufrechtBis = null};
            var bewilligung = new Foerderbewilligung {Freigabe = freigabe};
            var antrag = new Foerderantrag {Bewilligung = bewilligung};
            var service = new FoerderantragService();

            // Act
            var actual = service.IsAktiv(antrag, AnyStichtag);

            Assert.IsTrue(actual);
        }
    }
}
