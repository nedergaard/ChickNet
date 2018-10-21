﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickNet.Gate;

namespace ChickNet.UnitTests.GateTests
{
    public class MockPwnController : IPwmController
    {
        public List<DutyCycleChangeEvent> DutycycleHistory { get; }

        public MockPwnController()
        {
            DutycycleHistory = new List<DutyCycleChangeEvent>();
        }

        #region Implementation of IPwmController

        /// <inheritdoc />
        public Direction Direction { get; set; }

        /// <inheritdoc />
        public int DutyCycle { get; }

        /// <inheritdoc />
        public Task ChangeDutyCycleAsync(int percent)
        {
            DutycycleHistory.Add(
                new DutyCycleChangeEvent
                {
                    Tick = DateTime.Now.Ticks,
                    DutyCycle = percent,
                });
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class DutyCycleChangeEvent
    {
        public long Tick { get; set; }
        public int DutyCycle { get; set; }
    }
}
