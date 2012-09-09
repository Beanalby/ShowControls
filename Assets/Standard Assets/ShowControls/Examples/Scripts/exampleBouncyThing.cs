using UnityEngine;
using System.Collections;

/* These exist simply to show how the game pauses while
 * the fullscreen ShowControls are used. */
public class exampleBouncyThing : MonoBehaviour {
    public GameObject projector;

    void Update()
    {
        projector.transform.position = transform.position + Vector3.up * 1.24f;
        projector.transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.forward);
    }
    void OnCollisionEnter(Collision col)
    {
        rigidbody.velocity = col.contacts[0].normal * 5;
    }
}
