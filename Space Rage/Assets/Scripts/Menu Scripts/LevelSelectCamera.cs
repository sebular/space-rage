using UnityEngine;
using System.Collections;

public class LevelSelectCamera : MonoBehaviour {
	public float speed = 3;
	int LOWEST_LEVEL = 0;
	int HIGHEST_LEVEL = 7;

	int selectedIndex = 0;

	Vector3 fromPosition;
	Vector3 toPosition;

	bool sliding = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = 0;
		if (!sliding) {
			horizontal = Input.GetAxis("P1 Horizontal");
            if (horizontal == 0) horizontal = Input.GetAxis("P2 Horizontal");
		}

		if (horizontal > 0 && selectedIndex < HIGHEST_LEVEL) {
			selectedIndex ++;
			toPosition = new Vector3(selectedIndex * 2, 0, transform.position.z);
			Debug.Log(selectedIndex);
			sliding = true;
		}
		if (horizontal < 0 && selectedIndex > LOWEST_LEVEL) {
			selectedIndex --;
			toPosition = new Vector3(selectedIndex * 2, 0, transform.position.z);
			Debug.Log(selectedIndex);
			sliding = true;
		}

		if (sliding) slide();

		if (Input.GetButtonUp("P1 Fire1") || Input.GetButtonUp("P2 Fire1")) {
			Application.LoadLevel(selectedIndex + 1	);
		}
	}

	void slide() {
		transform.position = Vector3.Slerp(transform.position, toPosition, Time.deltaTime * speed);
		if (transform.position == toPosition) {
			Debug.Log("arrived");
			sliding = false;
		}
	}
}
