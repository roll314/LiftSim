using System;
using System.Collections.Generic;
using System.Text;

namespace LiftSim
{
    public interface IState
    {
        IEnumerable<IStateCondition> Ways { get; }
        string Name { get; }
        void OnStart(StateStorage stateStorage);
        void OnEnd(StateStorage stateStorage, IState state);
    }
}
