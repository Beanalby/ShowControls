using UnityEngine;
using System.Collections;

public class ShootableThing : MonoBehaviour {

    public GameObject hitEffect;

    public GameObject healthbarPrefab;
    public float healthbarOffset = .5f;
    public float healthbarWidth = 128;
    public float healthbarHeight = 15;
    
    public int MaxHealth;
    private int currentHealth;

    private GameObject healthbar;

	void Start () {
        Debug.Log("ShootableThing starting, prefab=" + healthbarPrefab);
        healthbar = (GameObject)Instantiate(healthbarPrefab, transform.position, transform.rotation);
        healthbar.GetComponent<HealthBar>().SetThing(this);
        currentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BulletHit(Bullet bullet)
    {
        currentHealth -= bullet.damage;
        if (currentHealth <= 0)
        {
            SendMessage("Die");
        }
        else
        {
            if (hitEffect != null)
                Instantiate(hitEffect, bullet.hitInfo.point, bullet.transform.rotation);
        }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
}
