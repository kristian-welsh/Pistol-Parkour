using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// removes any time delaying behaviour leaving the test client to extract and
// run delayed method when appropriate in the test environment
public class MockTimedAction : TimedAction
{
    public override void StartTimer()
    {
		
    }

    public override void PerformActionEarly()
    {
        delayedAction();
    }
}
