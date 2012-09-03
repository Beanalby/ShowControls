using UnityEngine;
using System.Collections;

class DudeMovement
{
    public Vector3 lookDir;
    public Vector3 movement;
    public bool turnNow;
    
    public DudeMovement() {
        Reset();
    }

    public void Reset()
    {
        lookDir = Vector3.zero;
        movement = Vector3.zero;
        turnNow = false;
    }
}

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(DudeWeaponController))]
public class DudeController : MonoBehaviour {

    public int MaxHealth = 100;

    private float moveSpeed = 10;
    private float turnSpeed = 10;

    private float gotHitLast = -100;
    private float gotHitCooldown = 1;

    private DudeMovement move;
    private CharacterController cc;
    private DudeWeaponController dwc;

    private int currentHealth;
	void Start () {
        move = new DudeMovement();
        cc = GetComponent<CharacterController>();
        dwc = GetComponent<DudeWeaponController>();
        currentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        move.Reset();
        HandleMovement();
        HandleFire();
        ApplyChanges();
	}

    void HandleMovement()
    {
        Vector3 newDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (dwc.IsFiring)
        {
            move.lookDir = dwc.FiringDir;
            move.turnNow = true;
        }
        else
            move.lookDir = newDir;
        move.movement = newDir * moveSpeed * Time.deltaTime;
    }

    void HandleFire()
    {
        if (!Input.GetButton("Fire1"))
            return;
    }

    void ApplyChanges()
    {
        cc.Move(move.movement);
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
        // let them know we run in to them.  They might
        // care (enemy) or they might not (wall).
        hit.gameObject.SendMessage("PlayerHit", gameObject, SendMessageOptions.DontRequireReceiver);
    }
}
