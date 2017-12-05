using System;
using System.Collections.Generic;
using System.Linq;

namespace LiftSim.States
{
    public class Idle : IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new Idle(), CheckForIdle),
            new StateConditionStub(new MotorOnUp(), CheckForMotorOnUp),
            new StateConditionStub(new MotorOnDown(), stateStorage => !CheckForMotorOnUp(stateStorage)),
            new StateConditionStub(new Idle(), storage => true)
        };

        private bool CheckForMotorOnUp(StateStorage stateStorage)
        {
            uint nearestFloor = (uint)Math.Round(stateStorage.YPosition / SettingsStorage.Settings.FLOOR_Y_SIZE);

            int upCount =
                stateStorage.LiftCalls.Count(lc => lc.Floor >= nearestFloor) +
                stateStorage.FloorCalls.Count(fc => fc >= nearestFloor);

            int totalCount = stateStorage.LiftCalls.Count + stateStorage.FloorCalls.Count;

            return totalCount / 2.0 <= upCount;
        }

        private bool CheckForIdle(StateStorage stateStorage)
        {
            return stateStorage.FloorCalls.Count == 0 && stateStorage.LiftCalls.Count == 0;
        }

        public string Name => "Idle";
        public void OnStart(StateStorage stateStorage)
        {
            
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
            
        }

    }
}