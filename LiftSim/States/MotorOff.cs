using System;
using System.Collections.Generic;
using System.Text;

namespace LiftSim.States
{
    public class MotorOff: IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new DoorsMotorOnOpen(), stateStorage => true)
        };

        public string Name => "MotorOff";
        public void OnStart(StateStorage stateStorage)
        {
            stateStorage.LastServesiedFloor = (uint)Math.Round(stateStorage.YPosition / SettingsStorage.Settings.FLOOR_Y_SIZE);
            Logger.Log("Motor is turned OFF", LogInfo.Debug);
            stateStorage.MotorInfo = MotorInfo.Idle;
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
        }
    }
}
