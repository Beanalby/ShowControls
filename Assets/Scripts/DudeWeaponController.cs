using UnityEngine;
using System.Collections;

public class DudeWeaponController : MonoBehaviour {

    public GameObject bullet;

    private float fireDelay = .1f;
    private float lastFire = -1;

    private GameObject target;

    private bool isFiring = false;
    private Vector3 firingDir = Vector3.zero;

	void Start () {
        target = GameObject.Find("Target");
    }
	
	// Update is called once per frame
	void Update () {
        if (!Input.GetButton("Fire1"))
        {
            isFiring = false;
            return;
        }
        isFiring = true;
        // figure out where the mouse is pointing
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;
        if (plane.Raycast(ray, out hitdist))
        {
            Vector3 point = ray.GetPoint(hitdist);
            firingDir = point - transform.position;
            if (lastFire + fireDelay > Time.time)
                return;

            lastFire = Time.time;
            // GameObject newBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            // figure out what the bullet hits now
            Vector3 rayStart = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            RaycastHit hit;
            if(Physics.Raycast(rayStart, point - transform.position, out hit))
            {
                if (target != null)
                    target.transform.position = hit.point;
                Quaternion bulletRot = Quaternion.LookRotation(hit.point - transform.position);
                Bullet newBullet = ((GameObject)Instantiate(bullet, transform.position, bulletRot)).GetComponent<Bullet>();
                newBullet.target = hit.collider.gameObject;
                newBullet.hitInfo = hit;
            }
        }
	}

    public bool IsFiring
    {
        get { return isFiring; }
    }
    public Vector3 FiringDir
    {
        get { return firingDir; }
    }
}
