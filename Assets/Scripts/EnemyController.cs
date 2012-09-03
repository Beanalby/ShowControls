using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public int damage;
    public float turnSpeed = 5f;
    public float moveSpeed = 1f;

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
        Vector3 moveAmount = Vector3.zero;
        // look at the dude (but not up or down)
        Vector3 dudeDir = dude.transform.position - transform.position;
        dudeDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(dudeDir);

        if (isActive)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
            moveAmount += (transform.forward * moveSpeed * Time.deltaTime);

        // kill a move that would put us outside our playpen
        // moveAmount.y = originalY - transform.position.y;
            moveAmount.y = 0;
        if (isActive)
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
