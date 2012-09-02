using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BulletHit(GameObject bullet)
    {
        Debug.Log("OW! - " + gameObject.name);
    }
}
