using System;

/* Represents a function that will be called in the near future.
 */
public interface ITimedAction
{
    /* Sets the function to be called
     */
    void AddDelayedAction(Action action);
    /* Starts the countdown to calling the function
     */
    void StartTimer();
    /* Cancels the countdown and calls the function immediately
     */
    void PerformActionEarly();
}
