using UnityEngine;
using System.Collections;

public class DudeWeaponController : MonoBehaviour {

    public static Vector3 AIM_NONE = new Vector3(-1000, -1000, -1000);
    public GameObject bullet;

    private float fireDelay = .1f;
    private float lastFire = -1;

    private GameObject target;

    private bool isFiring = false;
    private Vector3 firingDir = Vector3.zero;

    // when shooting, don't collide with the player.
    static private int rayMask = ~(1 << 8);
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
        /* figure out where the mouse is pointing.  If it's actually over
         * a shootable thing, aim at that.  Don't assume it's what we're
         * hitting just yet, there may be something else in the way. */
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 aimPoint = AIM_NONE;
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, rayMask))
            if (hit.collider.gameObject.GetComponent<ShootableThing>() != null)
                aimPoint = hit.collider.transform.position;
        if(aimPoint==AIM_NONE)
        {
            /* Aim at the position where the cursor intersects
             * the player's flat plane. */
            Plane plane = new Plane(Vector3.up, transform.position);
            float hitdist = 0.0f;
            if (plane.Raycast(cameraRay, out hitdist))
            {
                aimPoint = cameraRay.GetPoint(hitdist);
            }
        }
        if(aimPoint==AIM_NONE)
        {
            // should never happen!
            Debug.LogError("zero aimpoint past plane intersection");
            return;
        }
        firingDir = aimPoint - transform.position;
        /* we needed to calculate firingDir so we keep turning even if we're
         * not actually launching another bullet just yet.  NOW we can bail
         * if the firing isn't going to happen. */
        if (lastFire + fireDelay > Time.time)
            return;

        // figure out what the bullet hits now, instead of doing collision
        // detection
        lastFire = Time.time;
        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if(Physics.Raycast(rayStart, aimPoint - transform.position, out hit, Mathf.Infinity, rayMask))
        {
            Debug.Log("Ray along (" + (aimPoint - transform.position) + ") hit " + hit.collider.gameObject);
            //if (hit.collider.gameObject.name == "Floor")
            //    Debug.Break();
            if (target != null)
                target.transform.position = hit.point;
            Quaternion bulletRot = Quaternion.LookRotation(hit.point - transform.position);
            Bullet newBullet = ((GameObject)Instantiate(bullet, transform.position, bulletRot)).GetComponent<Bullet>();
            newBullet.target = hit.collider.gameObject;
            newBullet.hitInfo = hit;
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
