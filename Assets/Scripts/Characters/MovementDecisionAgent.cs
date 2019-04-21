using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MovementDecisionAgent
{
    void Recalculate(Vector3 velocity, Vector3 position);
    Vector3 CalculateMovementForce(Vector3 forward);
    bool wantsToJump();
}
