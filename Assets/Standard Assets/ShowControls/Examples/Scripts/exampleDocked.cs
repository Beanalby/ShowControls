using UnityEngine;
using System.Collections;

public class exampleDocked : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowControls.CreateDock(new ArrayList(new[] {
                new ControlItem("Test", KeyCode.E)}));
        }
	}
}
