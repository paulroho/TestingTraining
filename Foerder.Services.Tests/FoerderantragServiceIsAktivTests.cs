using System;
using System.Linq;
using FluentAssertions;
using Foerder.Services.Tests.TestDataBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Foerder.Services.Tests
{
    [TestClass]
    public class FoerderantragServiceIsAktivTests
    {
        private FoerderantragService _service;
        private Mock<IConfigurationProvider> _configProviderFake;
        private DateTime AnyStichtag { get; } = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            _configProviderFake = new Mock<IConfigurationProvider>();
            _service = new FoerderantragService(_configProviderFake.Object);
        }

        [TestMethod]
        public void WithoutBewilligung_ShouldConsiderStatesFromConfigByDataSourceAsAktiv()
        {
            var antrag = AFoerder.Antrag.WithoutBewilligung();
            antrag.DataSource = "MyDataSource";
            antrag.Status = "State1";
            _configProviderFake.Setup(p => p.GetStatusAntragAufrechtByDataSource("MyDataSource"))
                .Returns(new[] {"State1", "State2"}.ToList());

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            isAktiv.Should().BeTrue();
        }

        [TestMethod]
        public void WithoutBewilligung_ShouldConsiderStatesNotInConfigByDataSourceAsInaktiv()
        {
            var antrag = AFoerder.Antrag.WithoutBewilligung();
            antrag.DataSource = "MyDataSource";
            antrag.Status = "State Three";
            _configProviderFake.Setup(p => p.GetStatusAntragAufrechtByDataSource("MyDataSource"))
                .Returns(new[] { "State1", "State2" }.ToList());

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            isAktiv.Should().BeFalse();
        }

        [TestMethod]
        public void WithoutBewilligung_ShouldNotConsiderStatesInConfigForOtherDataSourceAsAktiv()
        {
            var antrag = AFoerder.Antrag.WithoutBewilligung();
            antrag.DataSource = "MyDataSource";
            antrag.Status = "State1";
            _configProviderFake.Setup(p => p.GetStatusAntragAufrechtByDataSource("AVeryDifferentDataSource"))
                .Returns(new[] { "State1", "State2" }.ToList());

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            isAktiv.Should().BeFalse();
        }

        [TestMethod]
        public void WithBewilligungAndUnrestrictedFreigabe_ShouldBeAktiv()
        {
            var antrag = AFoerder.Antrag.WithUnrestrictedFreigabe();

            // Act
            var isAktiv = _service.IsAktiv(antrag, AnyStichtag);

            isAktiv.Should().BeTrue();
        }

        [TestMethod]
        public void AtADateBeforeTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag:aufrechtBis.AddDays(-1));

            isAktiv.Should().BeTrue();
        }

        [TestMethod]
        public void AtTheFreigabeValidityDate_ShouldBeAktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis);

            isAktiv.Should().BeTrue();
        }

        [TestMethod]
        public void AtADateAfterTheFreigabeValidityDate_ShouldBeInaktiv()
        {
            var aufrechtBis = new DateTime(2017, 3, 1);
            var antrag = AFoerder.Antrag.WithFreigabeUntil(aufrechtBis);

            // Act
            var isAktiv = _service.IsAktiv(antrag, stichtag: aufrechtBis.AddDays(+1));

            isAktiv.Should().BeFalse();
        }
    }
}
