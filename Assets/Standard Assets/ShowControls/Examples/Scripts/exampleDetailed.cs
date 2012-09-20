using UnityEngine;
using System.Collections;

public class exampleDetailed : MonoBehaviour
{

    private ShowControls fsTop, fsBlender, fsUnity, bottomPerm, bottomTmp = null;

    private static KeyCode blenderKey = KeyCode.Tab;
    private static KeyCode unityKey = KeyCode.Z;
    private static KeyCode bottomTempKey = KeyCode.Space;
    private static KeyCode bottomPermKey = KeyCode.LeftControl;

    private static string descOrbit = "Orbit the camera around the selected object";
    private static string descPan = "Pan the camera in the view plane";
    private static string descZoom = "Zoom the view in & out";
    private static string descFocus = "Focus on the selected object";
    private static string descPlay = "Play/Stop the game or animation";
    private static string descFs = "Toggle fullscreen for the current view";
    private static string descBottomTemp = "This docked control slides (we don't remove slideSpeed), has a long duration (lots of text!), and automatically destroys itself when done - we create if if it's null, and don't explicitly destroy it.";
    private static string descBottomPerm = "This docked control has an infinite duration, and will stay until you hit Ctrl to remove it.  It also shows LEFT Ctrl, as opposed to the Unity/Blender screens which just show \"Ctrl\"";

    void Start ()
    {
        /* fsBlender is the fullscreen blender controls, which we show with Tab */
        fsBlender = ShowControls.CreateFullscreen(new[] {
            new ControlItem(descOrbit, MouseDirection.Both, MouseButton.MiddleClick),
            new ControlItem(descPan, KeyCode.LeftShift, MouseDirection.Both, MouseButton.MiddleClick),
            new ControlItem(descZoom, MouseButton.ScrollWheel),
            new ControlItem(descFocus, KeyCode.KeypadPeriod),
            new ControlItem(descPlay, new[] { KeyCode.LeftAlt, KeyCode.A }),
            new ControlItem(descFs, new[] { KeyCode.LeftShift, KeyCode.Space })
        });
        fsBlender.fullscreenTitle = "Handy Blender Controls";
        fsBlender.fullscreenClearKey = blenderKey;

        /* fsUnity is the fullscreen unity controls, which we show with Z */
        fsUnity = ShowControls.CreateFullscreen(new[] {
            new ControlItem(descOrbit, KeyCode.LeftAlt, MouseDirection.Both, MouseButton.LeftClick),
            new ControlItem(descPan, KeyCode.LeftAlt, MouseDirection.Both, MouseButton.MiddleClick),
            new ControlItem(descZoom, KeyCode.LeftAlt, MouseDirection.Horizontal, MouseButton.RightClick),
            new ControlItem(descFocus, KeyCode.F),
            new ControlItem(descPlay, new[] { KeyCode.LeftControl, KeyCode.P }),
            new ControlItem(descFs, KeyCode.Space)
        });
        fsUnity.fullscreenTitle = "Common controls for Unity";
        fsUnity.fullscreenClearKey = unityKey;

        /* fsTop is the top docked controls that always display, unless either
         * fullscreen display is showing. */
        fsTop = ShowControls.CreateDocked(new[] {
            new ControlItem("Show Blender controls", fsBlender.fullscreenClearKey),
            new ControlItem("Show Unity controls", fsUnity.fullscreenClearKey),
            new ControlItem("Create indefinite bottom dock", bottomPermKey),
            new ControlItem("Create temporary bottom dock", bottomTempKey)
        });
        fsTop.showDuration = -1;
        fsTop.size = ShowControlSize.Small;
        fsTop.slideSpeed = -1;
        fsTop.Show();

        /* the bottom controls are created now, but aren't displayed until
         * we press the proper key */
        bottomPerm = ShowControls.CreateDocked(new[] {
            new ControlItem(descBottomPerm, bottomPermKey)
        });
        bottomPerm.position = ShowControlPosition.Bottom;
        bottomPerm.hideLeftRightOnModifierKeys = false;
        bottomPerm.showDuration = -1;
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(fsBlender.fullscreenClearKey))
        {
            fsUnity.Hide();
            fsBlender.Toggle();
        }
        if (Input.GetKeyDown(fsUnity.fullscreenClearKey))
        {
            fsBlender.Hide();
            fsUnity.Toggle();
        }
        if (!fsBlender.IsShown && !fsUnity.IsShown)
        {
            fsTop.Show();
            /* if we're pressing the key for the temp ShowControls and
             * it doesn't already exist, instantiate it.  When it's done,
             * it'll automatically Destroy itself and we'll be able to
             * make it again. */
            if (Input.GetKeyDown(bottomTempKey) && bottomTmp == null)
            {
                bottomTmp = ShowControls.CreateDocked(new[] {
                    new ControlItem(descBottomTemp, bottomTempKey)
                });
                bottomTmp.offsetX = Screen.width / 2;
                bottomTmp.showDuration = 5;
                bottomTmp.position = ShowControlPosition.Bottom;
                bottomTmp.destroyWhenDone = true;
                bottomTmp.Show();
            }
            if (Input.GetKeyDown(bottomPermKey))
                bottomPerm.Toggle();
        }
        else
            fsTop.Hide();
	}
}
