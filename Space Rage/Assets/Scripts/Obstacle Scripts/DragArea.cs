using UnityEngine;
using System.Collections;

public class DragArea : MonoBehaviour {
	public float frictionAmount = 5;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player") {
			other.gameObject.SendMessage("beginDrag", frictionAmount);
		}
		
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.name =="Player") {
			other.gameObject.SendMessage("endDrag");
		}
	}
}
