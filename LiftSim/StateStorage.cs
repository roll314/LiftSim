using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiftSim
{
    public enum MotorInfo
    {
        Up,
        Down,
        Idle
    }

    public enum DoorsInfo
    {
        Opened,
        Closed
    }
    public enum LiftCallInfo
    {
        Up,
        Down
    }

    public class LiftCall
    {
        public LiftCallInfo Direction { get; set; }
        public uint Floor { get; set; }
    }

    public class StateStorage
    {
        public MotorInfo MotorInfo = MotorInfo.Idle;
        public uint LastServesiedFloor = 1;
        public DoorsInfo DoorsInfo = DoorsInfo.Closed;
        public double YPosition = 0;
        public List<LiftCall> LiftCalls = new List<LiftCall>();
        public List<uint> FloorCalls = new List<uint>();

        // todo change it to lastActionTime

        public DateTime LastOperationTime = DateTime.Now;

        public override string ToString()
        {
            return String.Format(
                   "MotorInfo       = {0} \n" +
                   "DoorsInfo       = {1} \n" +
                   "YPosition       = {2} \n" +
                   "LiftCallsCount  = {3} \n" +
                   "FloorCallsCount = {4} \n",
                   MotorInfo,
                   DoorsInfo,
                   YPosition,
                   LiftCalls.Count,
                   FloorCalls.Count
                   );
        }
    }
}