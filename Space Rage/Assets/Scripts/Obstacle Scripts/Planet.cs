using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
    GameObject player;
    public float gravity = 100;
	bool searchInitiated = false;
	bool searching = false;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (player) {
        	float gravityForce = gravity * 10000;
        	Vector3 forceVector = transform.position - player.transform.position;
        	player.rigidbody.AddForce(forceVector.normalized * (gravityForce / forceVector.magnitude) * Time.deltaTime);
		}
		else if (!searching && !searchInitiated) {
			this.Invoke ("startLookingForPlayer", 2);
			searchInitiated = true;
		}
		if (searching) searchForPlayer();
	}

	void startLookingForPlayer() {
		searching = true;
		searchInitiated = false;
	}

	void searchForPlayer() {
		player = GameObject.Find("Player");
		if (player) searching = false;
	}
}
