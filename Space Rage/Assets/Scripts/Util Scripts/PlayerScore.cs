using UnityEngine;
using System.Collections;
using System;

public class PlayerScore : MonoBehaviour {
    GameObject[] orbs = new GameObject[5];
    int currentLife = 5;
    int playerNumber;
    TextMesh text;


	// Use this for initialization
	void Start () {
        playerNumber = Int32.Parse(gameObject.name.Substring(gameObject.name.Length - 2));
        for (int i = 0; i < currentLife; i++)
        {
            GameObject orb = transform.Find("Health Orb " + (i + 1)).gameObject;
            orbs[i] = orb;
        }

        GameObject textObject = transform.Find("Text").gameObject;
        text = textObject.GetComponent<TextMesh>();
	}

    // Update is called once per frame
    void Update()
    {
        int lifeCount = Util.getPlayerLives(playerNumber - 1);
        if (lifeCount < currentLife)
        {
            insult();
        }
        currentLife = lifeCount;
	}

    public void setHealth(float health)
    {
        orbs[currentLife - 1].SendMessage("setHealth", health);
    }

    public void destroyOrb()
    {
        if (currentLife <= 0)
        {
            return;
        }
        HealthOrb h = orbs[currentLife - 1].GetComponent<HealthOrb>();
        HealthOrb h2 = orbs[currentLife].GetComponent<HealthOrb>();

        h.enabled = true;
        h2.enabled = false;
    }

    public void lose()
    {
        text.text = "YOU ARE A LOSER";
    }

    public void win()
    {
        text.text = "YOU WIN";
    }

    private void insult()
    {
        string[] insults = {
            "YOU ARE BAD",
            "THAT WAS TERRIBLE",
            "SERIOUSLY?",
            "OH MY GOD",
            "JUST STOP",
            "MISERABLE",
            "AWFUL",
            "BAD JOB",
            "WHY DID YOU DO THAT?",
            "YOU SHOULD FEEL BAD",
            "ARE YOU KIDDING?",
            "DO YOU NEED HELP?",
            "ARE YOU EVEN TRYING?",
            "LET SOMEONE ELSE PLAY",
            "POORLY DONE"
        };
        string currentText = text.text;
        while (currentText == text.text)
        {
            currentText = insults[UnityEngine.Random.Range(0, insults.Length - 1)];
        }
        text.text = currentText;
    }

    private void compliment()
    {

    }
}
