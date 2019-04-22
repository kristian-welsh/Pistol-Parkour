using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;

/* Runs integration tests against the Parkour and CharacterMovement classes, which were
 * very coupled at the time this test class was written, mainly with the intent to test parkour.
 */
public class ParkourTests
{
    // events published by parkour and movement in string form to assert against
    private System.Collections.Generic.List<String> eventsPublished;
    // gives test agent inputs to movement
    private MockMovementDecisionAgent agent;
    // one of the objects under test
    private CharacterMovement movement;
    // the other object under test
    private Parkour parkour;
    // a raycaster to provide fake results to stimulate movement
    private MockRaycaster movementRaycaster;
    // a raycaster to provide fake results to stimulate parkour
    private MockRaycaster parkourRaycaster;
    // a factory to allow parkour to generate timed actions during opperation
    private MockTimedActionFactory factory;

    /* Initializes the system under test
     */
    [SetUp]
    public void SetUp()
    {
        // initialize objects
        movementRaycaster = new MockRaycaster(0.1f);
        parkourRaycaster = new MockRaycaster(1f, "Climbable");
        agent = new MockMovementDecisionAgent();
        factory = new MockTimedActionFactory();
        movement = new CharacterMovement(agent, 10f, 5f, movementRaycaster);
        parkour = new Parkour(10, 3, 1, parkourRaycaster, factory);

        // subscribe to events
        movement.addForceEvent += RecieveAddForce;
        parkour.startParkourEvent += RecieveStartParkour;
        parkour.stopParkourEvent += RecieveStopParkour;

        // subscribe them to each other
        movement.RegisterEvents(parkour);
        parkour.RegisterEvents(movement);

        // initialize list to store the events
        eventsPublished = new List<String>();
    }

    /* Recieve an event from parkour
     */
    private void RecieveStartParkour(Vector3 normal, Vector3 velocity)
    {
        eventsPublished.Add("Start: " + normal + ", " + velocity);
    }

    /* Recieve an event from parkour
     */
    private void RecieveStopParkour()
    {
        eventsPublished.Add("Stop: , ");
    }

    /* Recieve an event from movement
     */
    private void RecieveAddForce(Vector3 force, ForceMode mode)
    {
        eventsPublished.Add("Force: " + force + ", " + mode);
    }

    /* Custom assertion comparing a list of events expected against events recieved.
     * This allows the tests to read much more clearly.
     */
    private void AssertEvents(String[,] events)
    {
        // for each expected event
        for(int i = 0; i < events.GetLength(0); i++)
        {
            // Assert that an event was recieved and is equal to the one expected
            String expectedEventString = events[i, 0] + ": " + events[i, 1] + ", " + events[i, 2];
            if(eventsPublished.Count < i + 1)
                Assert.Fail("Published events depleted, expected: " + expectedEventString);
            StringAssert.AreEqualIgnoringCase(expectedEventString, eventsPublished[i]);
        }
    }

    /* Run the system for a number of updates
     * Extracts data into a format usable by the system
     * provides the argument as a 2d array for ease of test readability
     */
    private void RunTicks(float[,] data)
    {
        for(int i = 0; i < data.GetLength(0); i++)
        {
            // extract data for this tick
            Vector3 velocity = new Vector3(data[i,0], data[i,1], data[i,2]);
            Vector3 forward = new Vector3(data[i,3], data[i,4], data[i,5]);
            Vector3 movementForce = new Vector3(data[i,6], data[i,7], data[i,8]);
            bool shouldJump = (data[i,9] == 1f);
            agent.addMovementForce(movementForce);
            agent.addJumpInput(shouldJump);
            // perform the update
            Tick(velocity, forward);
        }
    }

    /* Performs a single system update with a certain velocity and forward vector, at position 0, 0, 0
     */
    private void Tick(Vector3 velocity, Vector3 forward)
    {
        Vector3 position = Vector3.zero;
        movement.Recalculate(velocity, position, forward);
        parkour.ParkourCheck(forward, position, velocity, movement.HasClimbed, movement.Grounded);
    }

    // tests are named after what they test

    [Test]
    public void CanVerticalWallclimb()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall in front of us
        parkourRaycaster.addGroundResult(new Vector3(-1f, 0f, 0f));

        //move forward for one game tick and press jump
        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[1,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // manually perform the delayed action to test side-effects
        factory.currentAction.PerformActionEarly();

        // event name,   arg 1,          arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(-1.0, 0.0, 0.0)", "(0.0, 3.0, 0.0)"},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanRightWallrun()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall to our right
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, 1f));

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[1,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // manually perform the delayed action to test side-effects
        factory.currentAction.PerformActionEarly();

        // event name,   arg 1,          arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 0.0, 1.0)", "(3.0, 0.9, 0.0)"},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanLeftWallrun()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall to our left
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, -1f));

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[1,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // manually perform the delayed action to test side-effects
        factory.currentAction.PerformActionEarly();

        // event name,   arg 1,          arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 0.0, -1.0)", "(3.0, 0.9, 0.0)"},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanJumpFromWallclimb()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall in front of us
        parkourRaycaster.addGroundResult(new Vector3(-1f, 0f, 0f));

        // move forward for one game tick and press jump
        // then press jump again
        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // don't call delayedAction because jumping should call it, and we need to test for that.

        // event name,   arg 1,          arg 2
        AssertEvents(new String[5,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(-1.0, 0.0, 0.0)", "(0.0, 3.0, 0.0)"},
            {"Force", "(-5.0, 0.0, 0.0)", "Impulse"},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanJumpFromWallrun()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall to our right
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, 1f));

        // move forward for one game tick and press jump
        // then press jump again
        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // don't call delayedAction because jumping should call it, and we need to test for that.

        // event name,   arg 1,          arg 2
        AssertEvents(new String[5,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 0.0, 1.0)", "(3.0, 0.9, 0.0)"},
            {"Force", "(0.0, 0.0, 5.0)", "Impulse"},
            {"Stop", "", ""}
        });
    }
    
    // new feature
    // climb up a wall till you reach the top, then stop
    [Test]
    public void VerticalWallclimbCresting()
    {
        // we're on the ground
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        // there's a wall in front of us
        parkourRaycaster.addGroundResult(new Vector3(-1f, 0f, 0f));
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();// may be unnessecary with the way i implemented it

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},// jump forward
            {0f, 3f, 0f,   1f, 0f, 0f,   1f,0f,0f,   0f}// climb up (expected result of first tick)
        });
        // don't call delayedAction because cresting should call it, and we need to test for that.

        // event name,   arg 1,          arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(-1.0, 0.0, 0.0)", "(0.0, 3.0, 0.0)"},
            {"Stop", "", ""}
        });
        // Stop is the event we're most interested in, we'll get events depleted failure instead if the feature is unimplemented
    }
}
