using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Creates ITimedActions (used for dependancy injection to avoid static class reference)
 */
public interface TimedActionFactory
{
	ITimedAction Create(int seconds);
}

/* Creates TimedActions
 */
public class TimedActionFactoryImplementation : TimedActionFactory
{
	public ITimedAction Create(int seconds)
	{
		return TimedAction.Create(seconds);
	}
}
