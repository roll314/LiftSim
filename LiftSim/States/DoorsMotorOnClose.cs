using System;
using System.Collections.Generic;
using System.Linq;

namespace LiftSim.States
{
    public class DoorsMotorOnClose : IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new DoorsMotorOff(), CheckCanStopCloseDoors),
        };

        private bool CheckCanStopCloseDoors(StateStorage stateStorage)
        {
            return stateStorage.DoorsInfo == DoorsInfo.Opened && DateTime.Now - stateStorage.LastOperationTime >= SettingsStorage.Settings.DOORS_OPENING_TIME;
        }

        public string Name => "DoorsMotorOnClose";
        public void OnStart(StateStorage stateStorage)
        {
            stateStorage.LastOperationTime = DateTime.Now;
            Logger.Log("Doors are closing", LogInfo.Debug);
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
            if (state.Name == "DoorsMotorOff")
            {
                Logger.Log("Doors are closed", LogInfo.Info);
                stateStorage.LastOperationTime = DateTime.Now;
                stateStorage.DoorsInfo = DoorsInfo.Closed;

                stateStorage.FloorCalls.RemoveAll(fc => fc == stateStorage.LastServesiedFloor);
                stateStorage.LiftCalls.RemoveAll(lc => lc.Floor == stateStorage.LastServesiedFloor);
            }
        }
    }
}