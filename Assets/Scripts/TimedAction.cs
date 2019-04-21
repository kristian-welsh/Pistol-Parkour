using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Represents a function that will be called inthe near future
 * This is to be used in scripts, not to be attatched to gameobjects
 * Construct with Create, then attach functions to delayedAction with +=, then StartTimer.
 * TimedAction is not re-usable, one StartTimer per Create, necessary to avoid a memmory leak.
 * Many hoops are jumped through to allow the use of Start/Stop Coroutine
 */
public class TimedAction : MonoBehaviour, ITimedAction
{
    // The funciton to be called
    private Action action;
    // Number of seconds to wait before calling
    protected int seconds;
    // A reference to the currently waiting action (needed to cancel)
    private IEnumerator waitingAction;

    /* Sets the function to be called
     */
    public void AddDelayedAction(Action action)
    {
        this.action = action;
    }

    /* Statically create a usable object and return it.
     */
    public static TimedAction Create(int seconds)
    {
        TimedAction self = attach();
        self.seconds = seconds;
        return self;
    }

    /* Embeds the object in the scenegraph to allow use of Start/Stop coroutine
     */
    private static TimedAction attach()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject host = scene.GetRootGameObjects()[0];
        host.AddComponent<TimedAction>();
        return host.GetComponent<TimedAction>();
    }

    /* Starts the countdown to calling the function
     */
    public void StartTimer()
    {
        waitingAction = PerformAction();
        StartCoroutine(waitingAction);
    }

    /* Cancels the countdown and calls the function immediately
     */
    public void PerformActionEarly()
    {
        StopCoroutine(waitingAction);
        action();
        Destroy(this);
    }

    /* Waits a number of seconds then performs the action
     */
    private IEnumerator PerformAction()
    {
        yield return new WaitForSeconds(seconds);
        action();
        Destroy(this);
    }
}