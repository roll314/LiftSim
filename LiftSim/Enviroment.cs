using System;
using System.Collections.Generic;
using System.Text;

namespace LiftSim
{
    public class Enviroment
    {
        private DateTime _lastTick = DateTime.Now;
        public StateStorage StateStorage { get; private set; }
        public Enviroment(StateStorage stateStorage)
        {
            StateStorage = stateStorage;
        }

        public void ProcessPhysics()
        {
            TimeSpan deltaTime = DateTime.Now - _lastTick;

            if (StateStorage.MotorInfo == MotorInfo.Up)
            {
                StateStorage.YPosition += deltaTime.TotalMilliseconds * SettingsStorage.Settings.LIFT_Y_SPEED / 1000;
            }

            if (StateStorage.MotorInfo == MotorInfo.Down)
            {
                StateStorage.YPosition -= deltaTime.TotalMilliseconds * SettingsStorage.Settings.LIFT_Y_SPEED / 1000;
            }

            _lastTick = DateTime.Now;
        }
    }
}
