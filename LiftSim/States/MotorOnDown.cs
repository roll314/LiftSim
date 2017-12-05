using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSim.States
{
    public class MotorOnDown : IState
    {
        public IEnumerable<IStateCondition> Ways => new IStateCondition[]
        {
            new StateConditionStub(new MotorOff(), CheckForMotorOff),
            new StateConditionStub(new Idle(), CheckForIdle)
        };

        public string Name => "MotorOnDown";

        private bool CheckForMotorOff(StateStorage stateStorage)
        {
            List<uint> floorsForStop = GetFloorsForStop(stateStorage).OrderByDescending(fl => fl).ToList();
            uint nearFloorStop = floorsForStop.First();
            return stateStorage.YPosition <= (nearFloorStop * SettingsStorage.Settings.FLOOR_Y_SIZE);
        }

        private List<uint> GetFloorsForStop(StateStorage stateStorage)
        {
            uint nearestFloor = (uint)Math.Round(stateStorage.YPosition / SettingsStorage.Settings.FLOOR_Y_SIZE);

            List<uint> floorsForStop = stateStorage.LiftCalls.Where(lc => lc.Floor <= nearestFloor).Select(lc => lc.Floor).ToList();
            List<uint> floorCalls = stateStorage.FloorCalls.Where(fc => fc <= nearestFloor).ToList();

            floorsForStop.AddRange(floorCalls);

            return floorsForStop;
        }

        private bool CheckForIdle(StateStorage stateStorage)
        {
            return stateStorage.FloorCalls.Count == 0 && stateStorage.LiftCalls.Count == 0;
        }

        public void OnStart(StateStorage stateStorage)
        {
            stateStorage.MotorInfo = MotorInfo.Down;
            Logger.Log("Motor is turned Down ON", LogInfo.Debug);
        }

        public void OnEnd(StateStorage stateStorage, IState state)
        {
            
        }
    }
}
