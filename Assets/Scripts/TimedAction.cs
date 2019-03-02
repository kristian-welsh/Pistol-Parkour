using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* A timer that calls a function after some number of seconds
 * This is to be used in scripts, not to be attatched to gameobjects
 * Construct with Create, then attach functions to delayedAction with +=, then StartTimer.
 * TimedAction is not re-usable, one StartTimer per Create.
 */
class TimedAction : MonoBehaviour
{
    public delegate void DelayedAction();
    public DelayedAction delayedAction;
    
    protected int seconds;

    private IEnumerator waitingAction;

    public static TimedAction Create(int seconds)
    {
        TimedAction self = attach();
        self.seconds = seconds;
        return self;
    }

    private static TimedAction attach()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject host = scene.GetRootGameObjects()[0];
        host.AddComponent<TimedAction>();
        return host.GetComponent<TimedAction>();
    }

    public void StartTimer()
    {
        waitingAction = PerformAction();
        StartCoroutine(waitingAction);
    }

    public void PerformActionEarly()
    {
        StopCoroutine(waitingAction);
        delayedAction();
        Destroy(this);
    }

    private IEnumerator PerformAction()
    {
        yield return new WaitForSeconds(seconds);
        delayedAction();
        Destroy(this);
    }
}