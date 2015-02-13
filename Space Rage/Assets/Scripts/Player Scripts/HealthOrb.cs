using UnityEngine;
using System.Collections;
using System;

public class HealthOrb : MonoBehaviour {
    int playerNumber;
    float currentHealth;
    float maxHealth;
    bool playerDead;

	// Use this for initialization
	void Start () {
        playerNumber = Int32.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
	}
	
	// Update is called once per frame
	void Update () {
        if (!playerDead)
            transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(currentHealth / 100f, currentHealth / 100f, transform.localScale.z), Time.deltaTime * 5);
        else
            transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(currentHealth / 100f, currentHealth / 100f, transform.localScale.z), Time.deltaTime * 10);
	}

    public void setHealth(float health)
    {
        currentHealth = health;
        if (health == 0)
            playerDead = true;
    }
}
