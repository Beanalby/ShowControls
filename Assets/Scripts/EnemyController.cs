using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RigidbodyController))]
public class EnemyController : MonoBehaviour {

    public int damage;
    private bool isActive = true;
    DudeController dude;

    private RigidbodyController rc;

	void Start () {
        dude = GameObject.Find("Dude").GetComponent<DudeController>();
        rc = GetComponent<RigidbodyController>();
	}
	
	void FixedUpdate () {
        ChaseDude();    
	}

    void ChaseDude()
    {
        if (!isActive || dude == null)
            return;
        
        // look at the dude (but not up or down)
        Vector3 dudeDir = dude.transform.position - transform.position;
        dudeDir.y = 0;
        rc.TurnTowards(dudeDir);
        rc.Move(transform.forward);
    }

    public void Die()
    {
        // TODO something cool.
        Destroy(gameObject);
    }

    public void PlayerHit(Collision col)
    {
        // player bumped into us, HURT 'EM!
        dude.SendMessage("GotHit", damage);
    }
}