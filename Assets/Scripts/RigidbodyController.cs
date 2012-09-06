using UnityEngine;
using System.Collections;

/* Provides some CharacterController-like controls for rigidbodies */
[RequireComponent (typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour {

    static public int MASK_FLOORS = 1 << 9;
    public float turnSpeed = 5f;
    public float maxSpeed = 3f;
    public float accelerationGround = 10f;
    public float accelerationAir = 1f;
    public bool doJump = false;
    public float jumpSpeed = 10;
    public bool smoothTurn = true;

    public float idleFriction = 0;

    public bool IsGrounded()
    {
        return Physics.Raycast(collider.bounds.center, -Vector3.up, collider.bounds.extents.y + .1f, MASK_FLOORS);
    }
	// must be called from within FixedUpdate
    public void TurnTowards(Vector3 lookDir)
    {
        rigidbody.angularVelocity = Vector3.zero;
        if (lookDir == Vector3.zero)
        {
            return;
        }
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        if (smoothTurn)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        else
            transform.rotation = targetRot;
    }

    // must be called from within FixedUpdate
	public void Move(Vector3 moveDir)
    {
        float oldY = rigidbody.velocity.y;

        Vector3 oldVelocity = rigidbody.velocity;
        float oldSpeed = Vector3.Magnitude(rigidbody.velocity);
        Vector3 velocityChange = Vector3.zero;
        if (IsGrounded())
        {
            if (Vector3.Magnitude(moveDir) != 0)
            {
                velocityChange = moveDir * accelerationGround * Time.deltaTime;
            }
            else
            {
                if (idleFriction != 0)
                    velocityChange = -rigidbody.velocity * idleFriction;
            }
        }
        else
        {
            velocityChange = moveDir * accelerationAir * Time.deltaTime;
        }
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


        // apply jumping after the max capping
        float newY;
        if (doJump)
        {
            // the jump may occur just as they're landing; if that's the case,
            // they'll have big negative velocity (which is about to be cancelled
            // anyway).  Make sure their upward velocity is at least +jumpSpeed.
            newY = Mathf.Max(oldY + jumpSpeed, jumpSpeed);
            doJump = false;
        }
        else
        {
            // restore y to what it was before our direction modifying for control
            // we don't want holding forward to "pull down" right after jumping
            newY = oldY;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, newY, rigidbody.velocity.z);
	}
}