using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class ParkourTests
{
    private System.Collections.Generic.List<String> eventsPublished;
    private MockMovementDecisionAgent agent;
    private CharacterMovement movement;
    private Parkour parkour;
    private MockRaycaster movementRaycaster;
    private MockRaycaster parkourRaycaster;
    private MockTimedActionFactory factory;

    [SetUp]
    public void SetUp()
    {
        movementRaycaster = new MockRaycaster(0.1f);
        parkourRaycaster = new MockRaycaster(1f, "Climbable");
        agent = new MockMovementDecisionAgent();
        factory = new MockTimedActionFactory();
        movement = new CharacterMovement(agent, 10f, 5f, 3, movementRaycaster, factory);
        movement.startParkourEvent += RecieveStartParkour;
        movement.stopParkourEvent += RecieveStopParkour;
        movement.addForceEvent += RecieveAddForce;
        parkour = new Parkour(10, 3, parkourRaycaster);
        parkour.movement = movement;

        eventsPublished = new List<String>();
    }

    private void RecieveStartParkour(Vector3 velocity)
    {
        eventsPublished.Add("Start: " + velocity + ", ");
    }

    private void RecieveStopParkour()
    {
        eventsPublished.Add("Stop: , ");
    }

    private void RecieveAddForce(Vector3 force, ForceMode mode)
    {
        eventsPublished.Add("Force: " + force + ", " + mode);
    }

    private void AssertEvents(String[,] events)
    {
        for(int i = 0; i < events.GetLength(0); i++)
        {
            String expectedEventString = events[i, 0] + ": " + events[i, 1] + ", " + events[i, 2];
            if(eventsPublished.Count < i + 1)
                Assert.Fail("Published events depleted, expected: " + expectedEventString);
            StringAssert.AreEqualIgnoringCase(expectedEventString, eventsPublished[i]);
        }
    }

    private void RunTicks(float[,] data)
    {
        for(int i = 0; i < data.GetLength(0); i++)
        {
            Vector3 velocity = new Vector3(data[i,0], data[i,1], data[i,2]);
            Vector3 forward = new Vector3(data[i,3], data[i,4], data[i,5]);
            Vector3 movementForce = new Vector3(data[i,6], data[i,7], data[i,8]);
            bool shouldJump = (data[i,9] == 1f);
            agent.addMovementForce(movementForce);
            agent.addJumpInput(shouldJump);
            Tick(velocity, forward);
        }
    }

    private void Tick(Vector3 velocity, Vector3 forward)
    {
        Vector3 position = Vector3.zero;
        movement.Recalculate(velocity, position, forward);
        ParkourResult parkourResult = parkour.ParkourCheck(forward, position, velocity, movement.HasClimbed, movement.Grounded);
        if(parkourResult != null)
            movement.StartParkour(parkourResult.normal, parkourResult.velocity);
    }

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
        factory.currentAction.PerformActionEarly();

        // event name, arg 1, arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 3.0, 0.0)", ""},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanRightWallrun()
    {
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, 1f));

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[1,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        factory.currentAction.PerformActionEarly();

        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(3.0, 0.9, 0.0)", ""},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanLeftWallrun()
    {
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, -1f));

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[1,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        factory.currentAction.PerformActionEarly();

        AssertEvents(new String[4,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(3.0, 0.9, 0.0)", ""},
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

        //move forward for one game tick and press jump
        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // no manual delayed action, testing automatic one

        AssertEvents(new String[5,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 3.0, 0.0)", ""},
            {"Force", "(-5.0, 0.0, 0.0)", "Impulse"},
            {"Stop", "", ""}
        });
    }

    [Test]
    public void CanJumpFromWallrun()
    {
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        parkourRaycaster.addNullResult();
        parkourRaycaster.addGroundResult(new Vector3(0f, 0f, 1f));

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,10] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        // no manual delayed action, testing automatic one

        AssertEvents(new String[5,3] {
            {"Force", "(10.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(3.0, 0.9, 0.0)", ""},
            {"Force", "(0.0, 0.0, 5.0)", "Impulse"},
            {"Stop", "", ""}
        });
    }
    
/*
    // new feature
    // climb up a wall till you reach the top, then stop
    [Test]
    public void VerticalWallclimbCresting()
    {
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        parkourRaycaster.addGroundResult(new Vector3(-1f, 0f, 0f));
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();
        parkourRaycaster.addNullResult();

        //  velocity,      forward,  movementInput, jumpInput (bool)
        RunTicks(new float[2,13] {
            {2f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f},// jump forward
            {0f, 3f, 0f,   1f, 0f, 0f,   1f,0f,0f,   0f}// climb up (expected result of first tick)
        });
        // don't call delayedAction because cresting should call it, and we need to test for that.

        // event name, arg 1, arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(1.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(0.0, 3.0, 0.0)", ""},
            {"Stop", "", ""}
        });
        // Stop is the event we're most interested in, we'll get events depleted failure if it's not there
    }*/
}
