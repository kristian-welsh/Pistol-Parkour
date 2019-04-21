using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TimedActionFactory
{
	ITimedAction Create(int seconds);
}

public class TimedActionFactoryImplementation : TimedActionFactory
{
	public ITimedAction Create(int seconds)
	{
		return TimedAction.Create(seconds);
	}
}
