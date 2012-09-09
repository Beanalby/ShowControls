using UnityEngine;
using System.Collections;

public class examplePlayer : MonoBehaviour {

    public GameObject projector;

    /* The fullscreen ShowControls is created when the scene starts,
     * and persists through being toggled on & off.  We initialize it
     * with the "Move" ability, and add additional things to it
     * as they're collected. */
    ShowControls fullscreen = null;

    /* The "Die" ShowControls is created when needed, but it has an
     * infinite duration and is removed manually - namely, once
     * the user uses the ability.  We keep around this reference
     * so we'll have a way to hide it once they die. */
    ShowControls dieDock = null;

    /* The "Jump" ShowControls is a true "OneShot".  We create
     * it when needed, and it cleans up after itself when its
     * duration is done, so we don't need to keep it around
     * in any variable. */

    private string movePopup = "Moves around.  Touch the powerups to get more abilities";
    private string moveFull = "Move the player thingy around.  The game pauses while the fullscreen controls are shown, due to its pauseOnDisplay=True";
    private string jumpPopup = "You acquired the 'Jump' module!  Press 'Space' to jump!";
    private string jumpFull = "Jump.  Includes jumping mid-air.";
    private string diePopup = "You acquired the 'Die' module!  Left Click to Die.\nNote that this dock doesn't have a duration, and will stay around until you use the new ability.";
    private string dieFull = "Die.  Not the most useful ability.";

    private bool hasJump = false, hasDie = false, doJump = false;
    private float gravity = 20f;
    private Vector3 velocity = Vector3.zero;

    private bool canControl = true;
    private float dieStart = -1;
    Vector3 rotVelocity;

    private Vector3 startPoint;
    private CharacterController cc;

	void Start () {
        startPoint = transform.position;
        cc = GetComponent<CharacterController>();

        /* Create the fullscreen ShowControls with a control,
         * but don't show it yet. */
        fullscreen = ShowControls.CreateFullscreen(new ControlItem(moveFull, CustomDisplay.wasd));
        fullscreen.destroyWhenDone = false;

        // make a ShowControls to mention movement, and the powerups
        ShowControls m = ShowControls.CreateDocked(new ControlItem(movePopup, CustomDisplay.wasd));
        m.showDuration = 8;
        m.position = ShowControlPosition.Bottom;
        m.Show();
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            fullscreen.Toggle();

        projector.transform.position = transform.position + Vector3.up * 1.24f;
        projector.transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.forward);

        if (!canControl)
            return;
        if (hasJump && Input.GetButtonDown("Jump"))
            doJump = true;
        if (hasDie && Input.GetButtonDown("Fire1"))
            Die();
	}

    void Die()
    {
        /* If we haven't died yet, the dieDock will be showing.  We can
         * remove it now that the player knows how to use it. */
        if (dieDock.IsShown)
            dieDock.Hide();
        velocity.x = 0;
        velocity.y = 0;
        dieStart = Time.time;
        doJump = true;
        rotVelocity = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        canControl = false;
    }
    void FixedUpdate()
    {
        velocity.y -= (gravity* Time.deltaTime);
        if (canControl)
        {
            velocity.x = Input.GetAxis("Horizontal") * 5;
            velocity.z = Input.GetAxis("Vertical") * 5;
        }
        if (doJump)
        {
            velocity.y += 10;
            doJump = false;
        }

        cc.Move(velocity*Time.deltaTime);

        if (dieStart != -1)
        {
            transform.Rotate(rotVelocity);
            if (Time.time - dieStart >= 1)
            {
                cc.Move(startPoint - transform.position);
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                velocity = Vector3.zero;
                canControl = true;
                dieStart = -1;
            }
        }
    }

    void AddPowerup(string powerup)
    {
        // create a new docked ShowControls, and also add the control
        // to the fullscreen list.
        switch (powerup)
        {
            case "Jump":
                hasJump = true;
                fullscreen.controls.Add(new ControlItem(jumpFull, KeyCode.Space));
                /* create the "jump" dock.  We don't need to keep track of the
                 * ShowControls it returns, as it will take care of itself
                 * when the duration runs out.  Create it, & immediately show it. */
                ShowControls.CreateDocked(new ControlItem(jumpPopup, KeyCode.Space)).Show();
                break;
            case "Die":
                hasDie = true;
                fullscreen.controls.Add(new ControlItem(dieFull, MouseButton.LeftClick));
                /* Create the "die" dock.  We don't show it immediately because
                 * we want to adjust the duration before showing. */
                dieDock = ShowControls.CreateDocked(new ControlItem(diePopup, MouseButton.LeftClick));
                dieDock.showDuration = -1;
                dieDock.Show();
                break;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == 9)
            velocity.y = 0;
        examplePowerup p = hit.gameObject.GetComponent<examplePowerup>();
        if (p != null)
        {
            AddPowerup(p.powerupName);
            Destroy(hit.gameObject);
        }
    }
}
