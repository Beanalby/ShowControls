using UnityEngine;
using System.Collections;

class DudeMovement
{
    public Vector3 lookDir;
    public Vector3 newMovement;
    public Vector3 velocity;
    public bool turnNow;
    
    public DudeMovement() {
        Reset();
    }

    public void Reset()
    {
        lookDir = Vector3.zero;
        newMovement = Vector3.zero;
        turnNow = false;
    }
}

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(DudeWeaponController))]
public class DudeController : MonoBehaviour {

    public int MaxHealth = 100;

    private float maxMoveSpeed = 10;
    private float accelerationGround = 50;
    private float accelerationAir = 1;
    private float drag = 15f;
    private float turnSpeed = 10;

    private float gotHitLast = -100;
    private float gotHitCooldown = 1;

    private DudeMovement move;
    private CharacterController cc;
    private DudeWeaponController dwc;

    private float gravity = 10;

    private int currentHealth;
	void Start () {
        move = new DudeMovement();
        cc = GetComponent<CharacterController>();
        dwc = GetComponent<DudeWeaponController>();
        currentHealth = MaxHealth;
	}

    void Update()
    {
        HandleFire();
    }

    void FixedUpdate ()
    {
        move.Reset();
        HandleMovement();
        ApplyChanges();
	}

    void HandleMovement()
    {
        Vector3 newDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // move.newMovement = newDir * moveSpeed * Time.deltaTime;

        // adjust our velocity based on how we now want to move
        if (cc.isGrounded)
        {
            Vector3 change = newDir * accelerationGround * Time.deltaTime;
            //if (Vector3.Magnitude(change) != 0)
            //    Debug.Log("Changing " + move.velocity + " by " + change);
            move.velocity += change;
        }
        else
        {
            Vector3 change = newDir * accelerationAir * Time.deltaTime;
            //if (Vector3.Magnitude(change) != 0)
            //    Debug.Log("Air Changing " + move.velocity + " by " + change);
            move.velocity += change;
            move.velocity.y -= gravity * Time.deltaTime;
        }
        // if they didn't input a direction, apply some drag
        if(newDir.x== 0)
            move.velocity.x *= 1 - drag * Time.deltaTime;
        if(newDir.z==0)
            move.velocity.z *= 1 - drag * Time.deltaTime;

        // cap more movement speed based on the max
        move.velocity.x = Mathf.Min(maxMoveSpeed, Mathf.Max(-maxMoveSpeed, move.velocity.x));
        move.velocity.z = Mathf.Min(maxMoveSpeed, Mathf.Max(-maxMoveSpeed, move.velocity.z));

        // adjust our turn position
        if (dwc.IsFiring)
        {
            move.lookDir = dwc.FiringDir;
            move.turnNow = true;
        }
        else
            move.lookDir = newDir;
    }

    void HandleFire()
    {
        if (!Input.GetButton("Fire1"))
            return;
    }

    void ApplyChanges()
    {
        cc.Move(move.velocity * Time.deltaTime);
        if (Vector3.Magnitude(move.lookDir) != 0)
        {
            Quaternion targetRot = Quaternion.LookRotation(move.lookDir);
            if (move.turnNow)
                transform.rotation = targetRot;
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        }
    }

    public void GotHit(int damage)
    {
        // if they just got hit, skip it
        if (gotHitLast + gotHitCooldown > Time.time)
            return;

        currentHealth -= damage;
        gotHitLast = Time.time;
        if (currentHealth <= 0)
            Die();
        else
        {
            Debug.Log("OW, player's down to " + currentHealth);
            // TODO player got hit
        }
    }

    public void Die()
    {
        Debug.Log("BLARG, I ARE DEAD!");
        Destroy(gameObject);
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Floor")
            return;
        if (cc.isGrounded)
            move.velocity.y = 0;

        Debug.Log(name + " collided with" + hit.gameObject);
        // let them know we run in to them.  They might
        // care (enemy) or they might not (wall).
        hit.gameObject.SendMessage("PlayerHit", gameObject, SendMessageOptions.DontRequireReceiver);
    }
}
