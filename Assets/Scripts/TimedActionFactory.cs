using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TimedActionFactory
{
	TimedAction Create(int seconds);
}

public class TimedActionFactoryImplementation : TimedActionFactory
{
	public TimedAction Create(int seconds)
	{
		return TimedAction.Create(seconds);
	}
}
