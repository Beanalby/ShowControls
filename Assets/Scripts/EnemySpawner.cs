using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public float spawnCooldown = 5;

    private float lastSpawn = 0;

    public GameObject enemy;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (lastSpawn + spawnCooldown > Time.time)
            return;
        Instantiate(enemy, transform.position, transform.rotation);
        lastSpawn = Time.time;
	}
}
