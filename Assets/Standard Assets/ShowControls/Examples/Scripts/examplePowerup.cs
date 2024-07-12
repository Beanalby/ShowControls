using UnityEngine;
using System.Collections;
public class examplePowerup : MonoBehaviour {
    public string powerupName;
    void Start () {
        GetComponentInChildren<TextMesh>().text = powerupName;
    }
}
