using System;
public interface ITimedAction
{
    void AddDelayedAction(Action action);
    void StartTimer();
    void PerformActionEarly();
}
