using UnityEngine;
using System.Collections;

public class exampleSimple : MonoBehaviour {
    void Update () {
        string msgKey = "An example of showing controls for a single key "
            + "(Space, in this case)";
        string msgClick = "An example of showing a mouse click";
        string msgClickDrag = "An example of showing click & drag";
        string msgClickModifier = "An example of a mouse click "
            + "with a modifier key";
        string msgClickDragModifier = "Also an example of showing "
            + "multiple keys AND mouse button AND direction, "
            + "and showing multiple controls at once";

        if (Input.GetKeyDown(KeyCode.A)) {
            ShowControls sc = ShowControls.CreateDocked(
                new ControlItem(msgKey,
                    KeyCode.Space));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            ShowControls sc = ShowControls.CreateDocked(
                new ControlItem(msgClick,
                    MouseButton.LeftClick));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            ShowControls sc = ShowControls.CreateDocked(
                new ControlItem(msgClickDrag,
                    MouseDirection.Both,
                    MouseButton.RightClick));
            sc.destroyWhenDone = true;
            sc.Show();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            ShowControls sc = ShowControls.CreateDocked(
                new[] {
                    new ControlItem(msgClickModifier,
                        KeyCode.LeftControl,
                        MouseButton.LeftClick),
                    new ControlItem(msgClickDragModifier,
                        new[] {
                            KeyCode.LeftShift,
                            KeyCode.LeftControl
                        },
                    MouseDirection.Horizontal,
                    MouseButton.RightClick)
                });
            sc.destroyWhenDone = true;
            sc.Show();
        }
    }

    void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.wordWrap = true;
        style.normal.textColor = Color.white;
        GUI.Label(
            new Rect(Screen.width / 4, Screen.height / 4,
                Screen.width / 2, Screen.height / 2),
            "Press a, s, d, or f.\n\n"
            + "See exampleSimple.cs attached to the camera "
            + "for examples of how ShowControls can be invoked.\n\n"
            + "Note that we call .Show() immediately on the created "
            + "ShowControl objects.",
            style);
    }
}
