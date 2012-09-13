using UnityEngine;
using System.Collections;

public class exampleSimple : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ShowControls sc = null;
        if(Input.GetKeyDown(KeyCode.A))
            sc = ShowControls.CreateDocked(new ControlItem("An example of showing controls for a single key", KeyCode.Space));
        if (Input.GetKeyDown(KeyCode.S))
            sc = ShowControls.CreateDocked(new ControlItem("An example of showing a mouse click", MouseButton.LeftClick));
        if (Input.GetKeyDown(KeyCode.D))
            sc = ShowControls.CreateDocked(new ControlItem("An example of showing click & drag", MouseDirection.Both, MouseButton.RightClick));
        if (Input.GetKeyDown(KeyCode.F))
            sc = ShowControls.CreateDocked(new[] {
                new ControlItem("An example of a mouse click with a modifier key", KeyCode.LeftControl, MouseButton.LeftClick),
                new ControlItem("Also an example of showing multiple keys AND mouse button AND direction, and showing multiple controls at once", new[] { KeyCode.LeftShift, KeyCode.LeftControl }, MouseDirection.Horizontal, MouseButton.RightClick)
            });
        if (sc != null)
        {
            sc.destroyWhenDone = true;
            sc.Show();
        }
	}

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.wordWrap = true;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2),
            "Press a, s, d, or f.  They'll overwrap if you "
            + "mash keys, but presumably you'll be introducing new keys rarely enough "
            + "that they won't be immediately after each other.\n\n"
            + "Note that we call .Show() immediately on the returned ShowControl objects.\n\n"
            + "See exampleSceneDetailed for fullscreen, bottom docks, toggled instead of timed, & more.\n\n"
            + "Also see exampleSceneGame for ShowControls used in an (awful) game setting.",
            style);
    }
}
