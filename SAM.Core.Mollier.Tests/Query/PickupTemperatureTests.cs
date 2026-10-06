// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core.Mollier;
using Xunit;

namespace SAM.Core.Mollier.Tests.Query
{
    /// <summary>
    /// Covers all three Query.PickupTemperature overloads directly. FanProcessTests exercises the
    /// MollierPoint-extension overload only indirectly (as an input to FanProcess); these tests pin down its
    /// formula, its two siblings, and their NaN guards on their own.
    /// </summary>
    public class PickupTemperatureTests
    {
        private const double Pressure = 101325.0;
        private const double DryBulbTemperature = 16.0;
        private const double HumidityRatio = 0.008;
        private const double SpecificFanPower = 0.8;

        private static MollierPoint Point()
        {
            return new MollierPoint(DryBulbTemperature, HumidityRatio, Pressure);
        }

        [Fact]
        public void ByMollierPoint_MatchesHandCalculation_SfpOverRhoTimesCp()
        {
            // Formula: rise = sfp / (density * specificHeatCapacity), both evaluated at the given point.
            MollierPoint point = Point();

            double expected = SpecificFanPower / (point.Density() * SAM.Core.Mollier.Query.SpecificHeatCapacity_Air(point));
            double actual = point.PickupTemperature(SpecificFanPower);

            Assert.Equal(expected, actual, 9);
            Assert.InRange(actual, 0.5, 0.8); // sanity band for this inlet state
        }

        [Fact]
        public void ByMollierPoint_ScalesLinearlyWithSpecificFanPower()
        {
            MollierPoint point = Point();

            double single = point.PickupTemperature(SpecificFanPower);
            double doubled = point.PickupTemperature(SpecificFanPower * 2.0);

            Assert.Equal(single * 2.0, doubled, 9);
        }

        [Fact]
        public void ByMollierPoint_NullPoint_ReturnsNaN()
        {
            Assert.True(double.IsNaN(SAM.Core.Mollier.Query.PickupTemperature((MollierPoint)null, SpecificFanPower)));
        }

        [Fact]
        public void ByMollierPoint_NaNSpecificFanPower_ReturnsNaN()
        {
            Assert.True(double.IsNaN(Point().PickupTemperature(double.NaN)));
        }

        [Fact]
        public void Defaults_DelegateToThreeArgumentOverload_WithStandardAirProperties()
        {
            // PickupTemperature(sfp) forwards to PickupTemperature(sfp, 1.2, 1.005) - standard air density
            // [kg/m3] and specific heat capacity [kJ/kgK].
            double viaDefaults = SAM.Core.Mollier.Query.PickupTemperature(SpecificFanPower);
            double viaExplicit = SAM.Core.Mollier.Query.PickupTemperature(SpecificFanPower, 1.2, 1.005);

            Assert.Equal(viaExplicit, viaDefaults, 12);
        }

        [Fact]
        public void Defaults_NaNSpecificFanPower_ReturnsNaN()
        {
            Assert.True(double.IsNaN(SAM.Core.Mollier.Query.PickupTemperature(double.NaN)));
        }

        [Fact]
        public void ThreeArgument_MatchesItsImplementedFormula_SfpOverDensityTimesCp()
        {
            // Pins down the three-argument overload's CURRENT formula: sfp / density * specificHeatCapacity,
            // i.e. (sfp / density) * cp - evaluated left-to-right, cp multiplying rather than dividing.
            double density = 1.2;
            double specificHeatCapacity = 1.005;

            double expected = SpecificFanPower / density * specificHeatCapacity;
            double actual = SAM.Core.Mollier.Query.PickupTemperature(SpecificFanPower, density, specificHeatCapacity);

            Assert.Equal(expected, actual, 12);
        }

        [Fact]
        public void ThreeArgument_DiffersFromMollierPointOverload_WhenDensityAndCapacityAreNotBothOne()
        {
            // Documents a known discrepancy (left unresolved here - out of scope for the fan pickup fix):
            // the MollierPoint-extension overload divides by (density * cp), this overload divides by density
            // and then MULTIPLIES by cp, so the two are algebraically different whenever cp != 1/cp - they only
            // coincide when density*cp == density/cp, which is not the case for real air properties.
            double density = 1.2;
            double specificHeatCapacity = 1.005;

            double viaDensityAndCapacity = SAM.Core.Mollier.Query.PickupTemperature(SpecificFanPower, density, specificHeatCapacity);
            double viaMollierPointFormula = SpecificFanPower / (density * specificHeatCapacity);

            Assert.NotEqual(viaMollierPointFormula, viaDensityAndCapacity, 6);
        }

        [Theory]
        [InlineData(double.NaN, 1.2, 1.005)]
        [InlineData(0.8, double.NaN, 1.005)]
        [InlineData(0.8, 1.2, double.NaN)]
        public void ThreeArgument_AnyNaNArgument_ReturnsNaN(double sfp, double density, double specificHeatCapacity)
        {
            Assert.True(double.IsNaN(SAM.Core.Mollier.Query.PickupTemperature(sfp, density, specificHeatCapacity)));
        }
    }
}
