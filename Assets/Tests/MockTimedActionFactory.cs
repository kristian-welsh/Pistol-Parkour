using UnityEngine;

public class MockTimedActionFactory : TimedActionFactory
{
	public MockTimedAction currentAction;

	public TimedAction Create(int seconds)
	{
		currentAction = new MockTimedAction();
		return currentAction;
	}
}
