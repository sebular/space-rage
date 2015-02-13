using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    string type;
	// Use this for initialization
	void Start () {
        type = selectItemType();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.forward, Time.deltaTime * -60);
	}

	void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.name == "Player")
        {
            c.gameObject.SendMessage("pickUpItem", type);
            Destroy(gameObject);
        }
	}

    private string selectItemType()
    {
        return "missile";
    }
}
