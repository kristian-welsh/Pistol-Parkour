using UnityEngine;

public class MockTimedActionFactory : TimedActionFactory
{
	public MockTimedAction currentAction;

	public ITimedAction Create(int seconds)
	{
		currentAction = new MockTimedAction();
		return currentAction;
	}
}
