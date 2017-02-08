﻿using System;
using Foerder.Services.Tests.TestDataBuilders;
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
            var antrag = AFoerder.Antrag.WithoutBewilligung();

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsFalse(isAktiv);
        }

        [TestMethod]
        public void WithBewilligungAndUnrestrictedFreigabe_ShouldBeAktiv()
        {
            var antrag = AFoerder.Antrag.WithUnrestrictedFreigabe();

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtADateBeforeTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag:aufrechtBis.AddDays(-1));

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis);

            Assert.IsTrue(isAktiv);
        }

        [TestMethod]
        public void AtADateAfterTheFreigabeValidityDate_ShouldBeInaktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis.AddDays(+1));

            Assert.IsFalse(isAktiv);
        }
    }
}