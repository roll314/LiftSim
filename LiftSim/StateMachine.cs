using System;
using System.Diagnostics;

namespace LiftSim
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        private IState _previousState = null;
        private bool _debug;

        public StateStorage StateStorage { get; private set; }

        public StateMachine(IState initialState, StateStorage stateStorage, bool debug)
        {
            CurrentState = initialState;
            StateStorage = stateStorage;
            _debug = debug;
        }

        public void Refrech()
        {
            foreach (IStateCondition condition in CurrentState.Ways)
            {
                lock (StateStorage)
                {
                    if (!condition.Check(StateStorage)) continue;

                    if (_previousState != null)
                    {
                        if (_previousState.Name != "Idle" && _debug)
                        {
                            Logger.Log(string.Format("State was changed from {0} to {1}",
                                    CurrentState.Name,
                                    condition.State.Name), 
                                LogInfo.Debug);
                            Logger.Log("\n" + StateStorage, LogInfo.Debug);
                        }
                    }

                    CurrentState.OnEnd(StateStorage, condition.State);

                    _previousState = CurrentState;
                    
                    condition.State.OnStart(StateStorage);
                    CurrentState = condition.State;
                        
                    break;
                }
            }
        }
    }
}