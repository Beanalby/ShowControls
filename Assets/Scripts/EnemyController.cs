using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public int damage;
    private float turnSpeed = 5f;
    private float moveSpeed = 3f;

    private bool isActive = true;
    DudeController dude;
	// Use this for initialization
	void Start () {
        dude = GameObject.Find("Dude").GetComponent<DudeController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        ChaseDude();    
	}

    void ChaseDude()
    {
        if (!isActive || dude == null)
            return;

        Vector3 moveAmount = Vector3.zero;
        
        // look at the dude (but not up or down)
        Vector3 dudeDir = dude.transform.position - transform.position;
        dudeDir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dudeDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);

        moveAmount += (transform.forward * moveSpeed * Time.deltaTime);
        transform.position += moveAmount;
    }

    public void Die()
    {
        // TODO something cool.
        Destroy(gameObject);
    }

    public void PlayerHit(GameObject player)
    {
        // player bumped into us, HURT 'EM!
        player.SendMessage("GotHit", damage);
    }
}
