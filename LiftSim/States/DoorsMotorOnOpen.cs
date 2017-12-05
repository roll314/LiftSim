using System;
using System.Collections.Generic;

namespace LiftSim.States
{
    public class DoorsMotorOnOpen: IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new DoorsMotorOff(), CheckCanStopOpenDoors),
        };

        private bool CheckCanStopOpenDoors(StateStorage stateStorage)
        {
            return stateStorage.DoorsInfo == DoorsInfo.Closed && DateTime.Now - stateStorage.LastOperationTime >= SettingsStorage.Settings.DOORS_OPENING_TIME;
        }

        public string Name => "DoorsMotorOnOpen";
        public void OnStart(StateStorage stateStorage)
        {
            stateStorage.LastOperationTime = DateTime.Now;
            Logger.Log("Doors are opening", LogInfo.Debug);
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
            if (state.Name == "DoorsMotorOff")
            {
                Logger.Log("Doors are opened", LogInfo.Info);

                stateStorage.LastOperationTime = DateTime.Now;
                stateStorage.DoorsInfo = DoorsInfo.Opened;
            }
        }
    }
}