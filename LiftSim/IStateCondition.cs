using System;

namespace LiftSim
{
    public interface IStateCondition
    {
        IState State { get; }
        Func<StateStorage, bool> Check { get; }
    }

    public class StateConditionStub: IStateCondition
    {
        public StateConditionStub(IState state, Func<StateStorage, bool> check)
        {
            State = state;
            Check = check;
        }

        public IState State { get; }
        public Func<StateStorage, bool> Check { get; }


    }
}