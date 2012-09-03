using UnityEngine;
using System.Collections;

public class DudeModAngry : MonoBehaviour {

    public GameObject bulletBetter;

    private GameObject bulletNormal;

    DudeWeaponController dwc;
	// Use this for initialization
	void Start () {
        dwc = GetComponent<DudeWeaponController>();
        bulletNormal = dwc.bullet;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Turbo"))
            dwc.bullet = bulletBetter;
        else if (Input.GetButtonUp("Turbo"))
            dwc.bullet = bulletNormal;
	}
}
