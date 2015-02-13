using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour {
    Transform background;
    Transform bar;
    float currentHealth;
    float maxHealth;
    int playerNumber;

	// Use this for initialization
	void Start () {
        background = transform.Find("background");
        bar = transform.Find("bar");
        playerNumber = Int32.Parse( gameObject.name.Substring(gameObject.name.Length - 2) );
	}
	
	// Update is called once per frame
	void Update () {
        positionSelf();
        updateHealthBar();
	}

    public void setHealth(float health)
    {
        currentHealth = health;
        if (currentHealth > 100) currentHealth = 100;
        if (currentHealth < 0) currentHealth = 0;
        //Debug.Log(currentHealth);
    }

    void positionSelf()
    {
        float spacer = 20;
        if (playerNumber == 2)
        {
            spacer = 60;
        }
        float size = Camera.main.orthographicSize / 500;
        Vector3 paletteScale = new Vector3(size, size, 1);
        transform.localScale = paletteScale;

        Vector3 paletteLeftBorder = new Vector3(transform.position.x - (paletteScale.x / 2), transform.position.y, transform.position.z);
        Vector3 paletteTopBorder = new Vector3(transform.position.x, transform.position.y + (paletteScale.y / 2), transform.position.z);

        Vector3 paletteLeftScreen = Camera.main.WorldToScreenPoint(paletteLeftBorder);
        Vector3 paletteTopScreen = Camera.main.WorldToScreenPoint(paletteTopBorder);
        Vector3 paletteCenterScreen = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 palettePosition = Camera.main.ScreenToWorldPoint(new Vector3(paletteCenterScreen.x - paletteLeftScreen.x + spacer, (paletteTopScreen.y - paletteCenterScreen.y + spacer), 0));
        palettePosition.z = Camera.main.transform.position.z + 50;
        
        transform.position = palettePosition;
    }

    void updateHealthBar()
    {
        bar.localScale = new Vector3(currentHealth * 2, bar.localScale.y, bar.localScale.z);
        bar.localPosition = new Vector3(currentHealth, bar.localPosition.y, bar.localPosition.z);
    }
}
