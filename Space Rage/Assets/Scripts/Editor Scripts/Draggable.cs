using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
	bool dragging = false;
	public float percentage = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Tab)) {
			dragging = !dragging;
		}
		if (dragging) positionSlider();
	}

	void positionSlider() {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 relativePosition = mousePosition - transform.parent.position;
		float xPos = ( (relativePosition.x > 0) && (relativePosition.x < 400)) ? relativePosition.x : (relativePosition.x < 0) ? 0 : 400;
		this.transform.localPosition = new Vector3 (xPos, 0, transform.localPosition.z);
		percentage = xPos / 4;
	}
}
