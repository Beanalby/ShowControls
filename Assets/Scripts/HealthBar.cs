using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Texture healthTexture;
    public Texture missingTexture;

    private GameObject health;
    private GUITexture healthGui;
    private GameObject missing;
    private GUITexture missingGui;

    private float maxHeight, maxWidth;
    private float vertOffset;

    ShootableThing thing;

    public void SetThing(ShootableThing newthing)
    {
        thing = newthing;

        vertOffset = thing.healthbarOffset;
        maxWidth = thing.healthbarWidth;
        maxHeight = thing.healthbarHeight;

        health = new GameObject(thing.name + "Healthbar");
        health.transform.position = new Vector3(0, 0, 0);
        health.transform.localScale = new Vector3(0, 0, 0);

        health.AddComponent<GUITexture>();
        health.guiTexture.texture = healthTexture;
        health.guiTexture.pixelInset = new Rect(0, 0, healthTexture.width, healthTexture.height);
        healthGui = health.guiTexture;

        missing = new GameObject(thing.name + "HealthbarMissing");
        missing.transform.position = new Vector3(0, 0, 0);
        missing.transform.localScale = new Vector3(0, 0, 0);

        missing.AddComponent<GUITexture>();
        missing.guiTexture.texture = missingTexture;
        missing.guiTexture.pixelInset = new Rect(0, 0, missingTexture.width, missingTexture.height);
        missingGui = missing.guiTexture;
    }
	    
	// Update is called once per frame
	void Update () {
        if (thing.CurrentHealth >= thing.MaxHealth || thing.CurrentHealth <= 0)
        {
            healthGui.enabled = false;
            missingGui.enabled = false;
            return;
        }

        // scale the bar based on how far it is from the camera
        float scale = 30 / Vector3.Distance(thing.transform.position, Camera.main.transform.position);
        int width = (int)Mathf.Lerp(maxWidth / 3, maxWidth, scale);
        int height = (int)Mathf.Lerp(maxHeight/3, maxHeight, scale);
        float healthPercent =  (float)thing.CurrentHealth / (float)thing.MaxHealth;

        // position ourselves above our ShootableThing
        Vector3 targetPos = thing.transform.position;
        targetPos.y += vertOffset;
        Vector3 pos = Camera.main.WorldToViewportPoint(targetPos);
        float offset = ((float)(width / 2)) / Screen.width;
        pos.x -= offset;
        // don't let it go off the screen
        if (pos.x <= 5f / Screen.width)
            pos.x = 5f/Screen.width;
        if (pos.x + ((float)width / Screen.width) >= ((float)(Screen.width - 5) / Screen.width))
            pos.x = (Screen.width - (width+5)) / (float)Screen.width;

        health.transform.position = pos;
        missing.transform.position = pos;

        // update the LineRenderers with the health info
        missingGui.pixelInset = new Rect(0, 0, width, height);
        missingGui.enabled = true;

        float newWidth = (float)width * healthPercent;
        healthGui.pixelInset = new Rect(0, 0, newWidth, height);
        healthGui.enabled = true;
    }

    void OnDestroy()
    {
        Destroy(health);
        Destroy(missing);
    }
}
