using System;
using System.Collections.Generic;
using System.Linq;

namespace LiftSim.States
{
    public class DoorsMotorOff : IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new Idle(), CheckForIdle),
            new StateConditionStub(new DoorsMotorOnClose(), CheckCanCloseDoors),
            new StateConditionStub(new MotorOnUp(), stateStorage => CheckForMotorOnUp(stateStorage) && stateStorage.DoorsInfo == DoorsInfo.Closed),
            new StateConditionStub(new MotorOnDown(), stateStorage => !CheckForMotorOnUp(stateStorage) && stateStorage.DoorsInfo == DoorsInfo.Closed)
        };

        private bool CheckCanCloseDoors(StateStorage stateStorage)
        {
            return stateStorage.DoorsInfo == DoorsInfo.Opened && DateTime.Now - stateStorage.LastOperationTime >= SettingsStorage.Settings.DOORS_WAIT_TIME;
        }

        private bool CheckForIdle(StateStorage stateStorage)
        {
            return stateStorage.FloorCalls.Count == 0 && stateStorage.LiftCalls.Count == 0;
        }

        private bool CheckForMotorOnUp(StateStorage stateStorage)
        {
            uint nearestFloor = (uint)Math.Round(stateStorage.YPosition / SettingsStorage.Settings.FLOOR_Y_SIZE);

            int upCount =
                stateStorage.LiftCalls.Count(lc => lc.Floor >= nearestFloor) +
                stateStorage.FloorCalls.Count(fc => fc >= nearestFloor);

            if (upCount == 0) return false;

            int totalCount = stateStorage.LiftCalls.Count + stateStorage.FloorCalls.Count;

            return totalCount / 2.0 <= upCount;
        }

        public string Name => "DoorsMotorOff";
        public void OnStart(StateStorage stateStorage)
        {
            stateStorage.LastOperationTime = DateTime.Now;
            Logger.Log("Doors motor is turned OFF", LogInfo.Debug);
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
            if (state.Name == "DoorsMotorOnClose")
            {
                stateStorage.LastOperationTime = DateTime.Now;
            }
        }
    }
}