using UnityEngine;
using System.Collections;

/* Provides some CharacterController-like controls for rigidbodies */
[RequireComponent (typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour {

    private float turnSpeed = 5f;
    private float maxSpeed = 3f;
    private float acceleration = 10f;

	// must be called from within FixedUpdate
    public void TurnTowards(Vector3 lookDir)
    {
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
    }

    // must be called from within FixedUpdate
	public void Move(Vector3 moveDir)
    {
        float oldSpeed = Vector3.Magnitude(rigidbody.velocity);
        Vector3 velocityChange = moveDir * acceleration * Time.deltaTime;
        rigidbody.velocity += velocityChange;

        float newSpeed = Vector3.Magnitude(rigidbody.velocity);
        /* if they're going faster than max, don't allow them to go faster.
         * We DO want to allow acceleration in the opposite dir to slow down.
         * 
         * If they are attempting to speed up beyond max, we also want to leave
         * the velocity as high as it was - if they get thrown at high speed,
         * that speed shouldn't suddenly become capped because they
         * tried to move. */
        if (Vector3.Magnitude(rigidbody.velocity) > maxSpeed && newSpeed > oldSpeed)
        {
            // Debug.Log("NewSpeed " + newSpeed + " is too high, capping!");
            // at max velocity, let them change direction, but not magnitude
            rigidbody.velocity = Vector3.Normalize(rigidbody.velocity) * oldSpeed;
        }
	}
}