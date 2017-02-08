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
    }
}
