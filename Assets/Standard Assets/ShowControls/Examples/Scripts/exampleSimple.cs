using UnityEngine;
using System.Collections;

public class exampleSimple : MonoBehaviour
{
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowControls sc = ShowControls.CreateDocked(new ControlItem("An example of showing controls for a single key", KeyCode.Space));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowControls sc = ShowControls.CreateDocked(new ControlItem("An example of showing a mouse click", MouseButton.LeftClick));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowControls sc = ShowControls.CreateDocked(new ControlItem("An example of showing click & drag", MouseDirection.Both, MouseButton.RightClick));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowControls sc = ShowControls.CreateDocked(new[] {
                new ControlItem("An example of a mouse click with a modifier key", KeyCode.LeftControl, MouseButton.LeftClick),
                new ControlItem("Also an example of showing multiple keys AND mouse button AND direction, and showing multiple controls at once", new[] { KeyCode.LeftShift, KeyCode.LeftControl }, MouseDirection.Horizontal, MouseButton.RightClick)
            });
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
            "Press a, s, d, or f.  They'll overlap if you "
            + "mash keys, but presumably you'll be introducing new keys rarely enough "
            + "that they won't be immediately after each other.\n\n"
            + "Note that we call .Show() immediately on the returned ShowControl objects.\n\n",
            style);
    }
}
