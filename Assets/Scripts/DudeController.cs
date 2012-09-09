using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RigidbodyController))]
[RequireComponent (typeof(DudeWeaponController))]
public class DudeController : MonoBehaviour {

    public int MaxHealth = 100;

    private float gotHitLast = -100;
    private float gotHitCooldown = 1;

    private RigidbodyController rc;
    private DudeWeaponController dwc;

    private int currentHealth;

	void Start () {
        rc = GetComponent<RigidbodyController>();
        dwc = GetComponent<DudeWeaponController>();

        currentHealth = MaxHealth;
	}

    void Update()
    {
        if (Input.GetButtonDown("Jump") && rc.IsGrounded())
            rc.doJump = true;
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowControls.CreateDock(new ArrayList(new[] {
                new ControlItem("oneKey", KeyCode.Insert),
                new ControlItem("manyKey", new KeyCode[] { KeyCode.RightApple, KeyCode.LeftShift}),
                new ControlItem("oneDir", MouseDirection.Horizontal),
                new ControlItem("oneButton", MouseButton.LeftClick),
                new ControlItem("COMBO!", KeyCode.LeftShift, MouseDirection.Both, MouseButton.RightClick),
                new ControlItem("manyButton", MouseButton.BothClick),
                new ControlItem("oneDir+oneButton", MouseButton.ScrollWheel),
                new ControlItem("oneDir+manyButton", MouseDirection.Both, MouseButton.MiddleClick)
            }));
        }
    }

    void FixedUpdate ()
    {
        // appply gravity a second time for the player,
        // give less floaty jumps
        rigidbody.AddForce(Physics.gravity * rigidbody.mass);
        HandleMovement();
	}

    void HandleMovement()
    {
         Vector3 newDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (dwc.IsFiring)
        {
            rc.smoothTurn = false;
            rc.TurnTowards(dwc.FiringDir);
        }
        else
        {
            rc.smoothTurn = true;
            rc.TurnTowards(newDir);
        }
        rc.Move(newDir);
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
            GetComponentInChildren<TextMesh>().text = currentHealth.ToString();
            // TODO player got hit
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(Time.time + " " + name + " collided with " + col.gameObject.name);
        // let them decide if us hitting them is significant
        col.gameObject.SendMessage("PlayerHit", col, SendMessageOptions.DontRequireReceiver);
    }

    public void Die()
    {
        Debug.Log("BLARG, I ARE DEAD!");
        Destroy(gameObject);
    }
}
