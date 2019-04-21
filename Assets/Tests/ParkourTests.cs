using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class ParkourTests
{
    private System.Collections.Generic.List<String> eventsPublished;
    private TestableCharacterMovement movement;
    private Parkour parkour;
    private MockRaycaster movementRaycaster;
    private MockRaycaster parkourRaycaster;
    private MockTimedActionFactory factory;

    [SetUp]
    public void SetUp()
    {
        movementRaycaster = new MockRaycaster(0.1f);
        parkourRaycaster = new MockRaycaster(1f, "Climbable");
        factory = new MockTimedActionFactory();
        movement = new TestableCharacterMovement(5f, 5f, 3, movementRaycaster, factory);
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
            StringAssert.AreEqualIgnoringCase(expectedEventString, eventsPublished[i]);
        }
    }

    private void RunTicks(float[,] data)
    {
        for(int i = 0; i < data.GetLength(0); i++)
        {
            Vector3 velocity = new Vector3(data[i,0], data[i,1], data[i,2]);
            Vector3 position = new Vector3(data[i,3], data[i,4], data[i,5]);
            Vector3 forward = new Vector3(data[i,6], data[i,7], data[i,8]);
            Vector3 movementForce = new Vector3(data[i,9], data[i,10], data[i,11]);
            bool shouldJump = (data[i,12] == 1f);
            movement.addMovementForce(movementForce);
            movement.addJumpInput(shouldJump);
            Tick(velocity, position, forward);
        }
    }

    private void Tick(Vector3 velocity, Vector3 position, Vector3 forward)
    {
        movement.Recalculate(velocity, position, forward);
        ParkourResult parkourResult = parkour.ParkourCheck(forward, position, velocity, movement.HasClimbed, movement.Grounded);
        if(parkourResult != null)
            movement.StartParkour(parkourResult.normal, parkourResult.velocity);
    }

    [Test]
    public void CanVerticalWallclimb()
    {
        movementRaycaster.addGroundResult(new Vector3(0f, 1f, 0f));
        parkourRaycaster.addGroundResult(new Vector3(-1f, 0f, 0f));

        // velocity, position, forward, testMovementForce, wantsToJump (bool)
        RunTicks(new float[1,13] {
            {2f, 0f, 0f,   0f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        factory.currentAction.delayedAction();

        // event name, arg 1, arg 2
        AssertEvents(new String[4,3] {
            {"Force", "(1.0, 0.0, 0.0)", "Acceleration"},
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

        // velocity, position, forward, testMovementForce, wantsToJump (bool)
        RunTicks(new float[1,13] {
            {2f, 0f, 0f,   0f, 0f, 0f,   1f, 0f, 0f,   1f,0f,0f,   1f}
        });
        factory.currentAction.delayedAction();

        AssertEvents(new String[4,3] {
            {"Force", "(1.0, 0.0, 0.0)", "Acceleration"},
            {"Force", "(0.0, 5.0, 0.0)", "Impulse"},
            {"Start", "(3.0, 0.9, 0.0)", ""},
            {"Stop", "", ""}
        });
    }
}
