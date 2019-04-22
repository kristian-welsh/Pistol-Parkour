using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Interface for deciding in which direction to move and when to jump
 * Allows support for AI & Player characters with possible networked players in the future.
 * This is an extention of the controller responsibility for providing player input
 */
public interface MovementDecisionAgent
{
    // update decisions based on new information from physics engine
    void Recalculate(Vector3 velocity, Vector3 position);
    // which direciton to move (poorly named)
    Vector3 CalculateMovementForce(Vector3 forward);
    bool wantsToJump();
}
