using UnityEngine;
using System.Collections;

public class bouncyThing : MonoBehaviour {
    void OnCollisionEnter(Collision col)
    {
        rigidbody.velocity = col.contacts[0].normal * 5;
    }
}
