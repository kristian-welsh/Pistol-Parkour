using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/* Allows tests to trigger delayed actions early to assert on side-effects performed
 * removes any time delaying behaviour leaving the test client to extract and
 * run delayed method when appropriate in the test environment without waiting in real time
 */
public class MockTimedAction : ITimedAction
{
    private Action action;

    public void AddDelayedAction(Action action)
    {
        this.action = action;
    }

    public void StartTimer()
    {
		
    }

    public void PerformActionEarly()
    {
        action();
    }
}
