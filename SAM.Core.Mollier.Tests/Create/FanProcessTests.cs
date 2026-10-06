// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core.Mollier;
using Xunit;

namespace SAM.Core.Mollier.Tests.Create
{
    /// <summary>
    /// Covers Create.FanProcess(MollierPoint, double): the specific fan power produces a temperature RISE
    /// (PickupTemperature [K]) that is added to the inlet dry bulb temperature [C], leaving humidity ratio and
    /// pressure unchanged.
    /// </summary>
    public class FanProcessTests
    {
        private const double Pressure = 101325.0;
        private const double InletDryBulbTemperature = 16.0;
        private const double HumidityRatio = 0.008;
        private const double SpecificFanPower = 0.8;
        private const double Tolerance = 1e-9;

        private static MollierPoint Inlet()
        {
            return new MollierPoint(InletDryBulbTemperature, HumidityRatio, Pressure);
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_AddsPickupTemperatureToInletDryBulbTemperature()
        {
            MollierPoint inlet = Inlet();
            double pickupTemperature = inlet.PickupTemperature(SpecificFanPower);

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);

            Assert.NotNull(fanProcess);
            Assert.Equal(InletDryBulbTemperature, fanProcess.Start.DryBulbTemperature, 9);
            Assert.True(fanProcess.End.DryBulbTemperature > fanProcess.Start.DryBulbTemperature,
                "A fan dissipates its power into the air stream, so the outlet must be warmer than the inlet");
            Assert.Equal(InletDryBulbTemperature + pickupTemperature, fanProcess.End.DryBulbTemperature, 9);
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_TemperatureRise_EqualsPickupTemperature()
        {
            MollierPoint inlet = Inlet();
            double pickupTemperature = inlet.PickupTemperature(SpecificFanPower);

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);

            Assert.NotNull(fanProcess);
            Assert.Equal(pickupTemperature, fanProcess.End.DryBulbTemperature - fanProcess.Start.DryBulbTemperature, 9);
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_DoesNotTreatPickupTemperatureAsAbsoluteTemperature()
        {
            // Regression guard for the original defect: the outlet dry bulb temperature was set to the pickup
            // temperature itself (a ~0.65 K rise became a 0.65 C outlet from a 16 C inlet), giving a negative
            // temperature rise. This assertion fails under that implementation.
            MollierPoint inlet = Inlet();
            double pickupTemperature = inlet.PickupTemperature(SpecificFanPower);

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);

            Assert.NotNull(fanProcess);
            Assert.True(System.Math.Abs(fanProcess.End.DryBulbTemperature - pickupTemperature) > 1.0,
                "The outlet dry bulb temperature must not be the pickup temperature rise treated as an absolute temperature");
            Assert.InRange(fanProcess.End.DryBulbTemperature, 16.0, 17.0);
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_PreservesHumidityRatioAndPressure()
        {
            // A fan adds sensible heat only: no moisture is added or removed, and the process is defined at a
            // single pressure.
            MollierPoint inlet = Inlet();

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);

            Assert.NotNull(fanProcess);
            Assert.Equal(HumidityRatio, fanProcess.Start.HumidityRatio, 9);
            Assert.Equal(HumidityRatio, fanProcess.End.HumidityRatio, 9);
            Assert.Equal(Pressure, fanProcess.Start.Pressure, 6);
            Assert.Equal(Pressure, fanProcess.End.Pressure, 6);
            Assert.True(fanProcess.End.IsValid(), "The outlet state must be a valid Mollier point");
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_MatchesFanProcess_ByDryBulbTemperature()
        {
            // Factory consistency: asking for the outlet temperature that the specific fan power implies must
            // produce the same states as the explicit dry bulb overload.
            MollierPoint inlet = Inlet();
            double pickupTemperature = inlet.PickupTemperature(SpecificFanPower);

            FanProcess bySpecificFanPower = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);
            FanProcess byDryBulbTemperature = SAM.Core.Mollier.Create.FanProcess_ByDryBulbTemperature(
                inlet, InletDryBulbTemperature + pickupTemperature);

            Assert.NotNull(bySpecificFanPower);
            Assert.NotNull(byDryBulbTemperature);
            Assert.Equal(byDryBulbTemperature.End.DryBulbTemperature, bySpecificFanPower.End.DryBulbTemperature, 9);
            Assert.Equal(byDryBulbTemperature.End.HumidityRatio, bySpecificFanPower.End.HumidityRatio, 9);
            Assert.Equal(byDryBulbTemperature.End.Pressure, bySpecificFanPower.End.Pressure, 6);
        }

        [Fact]
        public void FanProcess_BySpecificFanPower_ScalesLinearlyWithSpecificFanPower()
        {
            // The pickup is proportional to the specific fan power at a fixed inlet state, so twice the power
            // gives twice the rise.
            MollierPoint inlet = Inlet();

            FanProcess single = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower);
            FanProcess doubled = SAM.Core.Mollier.Create.FanProcess(inlet, SpecificFanPower * 2.0);

            double singleRise = single.End.DryBulbTemperature - single.Start.DryBulbTemperature;
            double doubledRise = doubled.End.DryBulbTemperature - doubled.Start.DryBulbTemperature;

            Assert.Equal(singleRise * 2.0, doubledRise, 9);
        }

        [Fact]
        public void FanProcess_ZeroSpecificFanPower_LeavesTheAirStateUnchanged()
        {
            // No fan power, no pickup: the outlet equals the inlet rather than collapsing to 0 C as it did when
            // the rise was treated as an absolute temperature.
            MollierPoint inlet = Inlet();

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, 0.0);

            Assert.NotNull(fanProcess);
            Assert.Equal(InletDryBulbTemperature, fanProcess.End.DryBulbTemperature, 9);
            Assert.Equal(HumidityRatio, fanProcess.End.HumidityRatio, 9);
        }

        [Fact]
        public void FanProcess_NegativeSpecificFanPower_GivesANegativeRise()
        {
            // A negative specific fan power is unphysical, but the factory stays consistent with
            // End = Start + PickupTemperature rather than producing an unrelated absolute temperature.
            MollierPoint inlet = Inlet();
            double pickupTemperature = inlet.PickupTemperature(-SpecificFanPower);

            FanProcess fanProcess = SAM.Core.Mollier.Create.FanProcess(inlet, -SpecificFanPower);

            Assert.NotNull(fanProcess);
            Assert.True(pickupTemperature < 0);
            Assert.Equal(InletDryBulbTemperature + pickupTemperature, fanProcess.End.DryBulbTemperature, 9);
        }

        [Fact]
        public void FanProcess_NullInlet_ReturnsNull()
        {
            Assert.Null(SAM.Core.Mollier.Create.FanProcess((MollierPoint)null, SpecificFanPower));
        }

        [Fact]
        public void FanProcess_NaNSpecificFanPower_ReturnsNull()
        {
            Assert.Null(SAM.Core.Mollier.Create.FanProcess(Inlet(), double.NaN));
        }

        [Fact]
        public void FanProcess_InvalidInlet_ReturnsNull()
        {
            // An inlet whose pickup temperature cannot be evaluated yields no process, consistent with the
            // factory returning null for a null inlet or a NaN specific fan power.
            MollierPoint invalidInlet = new MollierPoint(double.NaN, HumidityRatio, Pressure);
            Assert.False(invalidInlet.IsValid());

            Assert.Null(SAM.Core.Mollier.Create.FanProcess(invalidInlet, SpecificFanPower));
        }
    }
}
