using UnityEngine;
using System.Collections;

public class exampleDocked : MonoBehaviour {

    ShowControls sc = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (sc == null && Input.GetKeyDown(KeyCode.Tab))
        {
            sc = ShowControls.CreateFullscreen(new ArrayList(new[] {
                new ControlItem("Move", CustomDisplay.arrows),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E),
                new ControlItem("Test", KeyCode.E)
            }));
        }
	}
}
