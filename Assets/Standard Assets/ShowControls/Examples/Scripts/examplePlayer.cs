using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class examplePlayer : MonoBehaviour {
    public GameObject projector;

    // The movement controls show at the bottom of the screen
    // permanently, unless the fullscreen is being shown
    // (both are toggled at the same time)
    ShowControls bottomDock = null;

    // The fullscreen ShowControls is created when the scene starts,
    // and persists through being toggled on & off.  We initialize it
    // with the "Move" ability, and add additional things to it
    // as they're collected.
    ShowControls fullscreen = null;

    // The "Die" ShowControls is created when needed, but it has an
    // infinite duration and is removed manually - namely, once
    // the user uses the ability.  We keep around this reference
    // so we'll have a way to hide it once they die.
    ShowControls dieDock = null;

    // These "jump" controls show when the player is inside the green
    // volume.  Normally volumes like this would be invisible,
    // but we gave it a green material so we can see what's going on.
    ShowControls jumpControlsFromTrigger = null;

    // The "Jump" ShowControls on pickup is a true "OneShot".  We create
    // it when needed, and it cleans up after itself when its
    // duration is done, so we don't need to keep it around
    // in any variable.

    // the text we'll show in various showControls widgets
    Dictionary<string,string> scText = new Dictionary<string, string>() {
        {"movePopup",  "Moves around.  Touch the powerups to get more abilities."},
        {"moveFull",  "Move the player thingy around.  The game pauses while the fullscreen controls are shown, due to its pauseOnDisplay=True\nMore things will be added here as you get them."},
        {"menuPopup",  "View all the controls.  This dock is ShowControlSize.Small"},
        {"jumpPopup",  "You acquired the 'Jump'!  This has the default dock behavior, so it disappears automatically after 3 seconds."},
        {"jumpFullscreen",  "Jump.  Includes jumping mid-air."},
        {"jumpTrigger",  "Jump up to platform!  This keeps displaying as long as the user is inside the green trigger, ensuring they'll be able to read it at their leisure."},
        {"diePopup",  "You acquired the 'Die' module!\nNote that this dock has duration=-1, so it will stay until you Die.  Good way to make sure users see & try the new stuff." },
        {"dieFullscreen",  "Die.  Not the most useful ability."},
    };

    //various bits for the actual gameplay of the scene
    private bool hasJump = false, hasDie = false, doJump = false;
    public bool HasJump { get { return hasJump; } }
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

        // Create the fullscreen ShowControls with a control,
        // but don't show it yet.
        fullscreen = ShowControls.CreateFullscreen(
            new ControlItem((string)scText["moveFull"], CustomDisplay.wasd));
        fullscreen.fullscreenMessageLeft = "Mash ";
        fullscreen.fullscreenClearKey = KeyCode.Tab;
        fullscreen.fullscreenMessageRight = "to keep rockin'";

        // make a ShowControls at the bottom to show movement &
        // the controls screen.  It stays around forever.
        bottomDock = ShowControls.CreateDocked(
            new[] {
                new ControlItem(scText["movePopup"], CustomDisplay.wasd),
                new ControlItem(scText["menuPopup"], KeyCode.Tab)
            });
        bottomDock.size = ShowControlSize.Small;
        bottomDock.showDuration = -1;
        bottomDock.slideSpeed = -1;
        bottomDock.position = ShowControlPosition.Bottom;
        bottomDock.Show();

        // intiailize jumpControlsFromTrigger now, although
        // we only show it when the player's inside the trigger
        jumpControlsFromTrigger = ShowControls.CreateDocked(new ControlItem(scText["jumpTrigger"], KeyCode.Space));
        jumpControlsFromTrigger.offsetX = Screen.width / 2;
        jumpControlsFromTrigger.showDuration = -1;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            // if we're showing the fullscreen, don't show the bottom dock,
            // & vice versa
            bottomDock.Toggle();
            fullscreen.Toggle();
        }
        if(projector) {
            projector.transform.position = transform.position + Vector3.up * 1.24f;
            projector.transform.rotation = Quaternion.LookRotation(-Vector3.up,
                transform.forward);
        }

        if (!canControl) {
            return;
        }
        if (hasJump && Input.GetButtonDown("Jump"))
            doJump = true;
        if (hasDie && Input.GetButtonDown("Fire1"))
            Die();
    }

    void Die() {
        // If we haven't died yet, the dieDock will be showing.  We can
        // remove it now that the player knows how to use it.
        if (dieDock.IsShown)
            dieDock.Hide();
        velocity.x = 0;
        velocity.y = 0;
        dieStart = Time.time;
        doJump = true;
        rotVelocity = new Vector3(Random.Range(-3f, 3f),
            Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        canControl = false;
    }
    void FixedUpdate() {
        velocity.y -= (gravity* Time.deltaTime);
        if (canControl) {
            velocity.x = Input.GetAxis("Horizontal") * 5;
            velocity.z = Input.GetAxis("Vertical") * 5;
        }
        if (doJump) {
            velocity.y = 10;
            doJump = false;
        }

        cc.Move(velocity*Time.deltaTime);

        if (dieStart != -1) {
            transform.Rotate(rotVelocity);
            if (Time.time - dieStart >= 1) {
                cc.Move(startPoint - transform.position);
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                velocity = Vector3.zero;
                canControl = true;
                dieStart = -1;
            }
        }
    }

    void AddPowerup(string powerup) {
        // create a new docked ShowControls, and also add the control
        // to the fullscreen list.
        switch (powerup) {
            case "Jump":
                hasJump = true;
                // create the "jump" dock.  We don't need to keep track of the
                // ShowControls it returns, as it will take care of itself
                // when the duration runs out.  Create it, & immediately
                // show it.
                ShowControls.CreateDocked(
                    new ControlItem(scText["jumpPopup"], KeyCode.Space)).Show();
                // also add the "Jump" ability to the fullscreen controls
                fullscreen.controls.Add(
                    new ControlItem(scText["jumpFullscreen"], KeyCode.Space));
                break;
            case "Die":
                hasDie = true;
                // Create the "die" dock.  Unlike "jump" above, it has an infinite
                // duration, because it lasts until the player uses it
                dieDock = ShowControls.CreateDocked(
                    new ControlItem(scText["diePopup"], KeyCode.LeftControl));
                dieDock.showDuration = -1;
                dieDock.Show();
                // also add the "Die" ability to the fullscreen controls
                fullscreen.controls.Add(
                    new ControlItem(scText["dieFullscreen"], KeyCode.LeftControl));
                break;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.layer == 9) { return; } // floor
        examplePowerup p = hit.gameObject.GetComponent<examplePowerup>();
        if(p != null) {
            AddPowerup(p.powerupName);
            Destroy(hit.gameObject);
        }
    }


    // show/hide jumpControlsFromTrigger when they're in the green volume
    void OnTriggerEnter(Collider other) {
        if(hasJump) {
            jumpControlsFromTrigger.Show();
        }
    }
    void OnTriggerExit(Collider other) {
        jumpControlsFromTrigger.Hide();
    }
}