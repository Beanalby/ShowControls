using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int damage = 10;
    public float distance = 0;
    public GameObject target;
    public RaycastHit hitInfo;
    private float speed = 150f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = transform.forward * Time.deltaTime * speed;
        if (distance + Vector3.Magnitude(movement) > hitInfo.distance)
        {
            Debug.Log("Sending BulletHit to " + hitInfo.collider.gameObject);
            hitInfo.collider.gameObject.SendMessage("BulletHit", this, SendMessageOptions.RequireReceiver);
            Destroy(gameObject);
        }
        else
        {
            transform.position += movement;
            distance += Vector3.Magnitude(movement);
        }
	}
}
