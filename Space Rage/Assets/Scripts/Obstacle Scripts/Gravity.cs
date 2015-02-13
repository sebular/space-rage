using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gravity : MonoBehaviour {
	public float direction = 0;
	public float magnitude = 100;
	bool pushing = false;
	GameObject player;
	Vector3 forceVector;
	float oldDirection = 0;
	float oldMagnitude = 100;
	public List<GameObject> pushTargets = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//player = GameObject.Find("Player");
		//pushTargets.Add(player);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (pushing) {
			//if (!player) {
				//player = GameObject.Find("Player");
				//pushTargets.Add(player);
			//}
			foreach (GameObject g in pushTargets) {
				Push(g);
			}
		}
	}

	void Update () {
		pushTargets.RemoveAll(isNotNull);
	}

	private static bool isNotNull(GameObject g) {
		if (!g) return true;
		return false;
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Piece" || other.gameObject.name == "Player")
        {
            if (!pushTargets.Contains(other.gameObject))
			    pushTargets.Add(other.gameObject);
		}
		beginPush();
	}
	
	void OnTriggerExit(Collider other)
	{

	}

	void Push(GameObject o) {
		if (o)
		{
			if (direction != oldDirection || magnitude != oldMagnitude)
			{
				forceVector = getForceVector();
			}
			o.rigidbody.AddForce(forceVector * Time.deltaTime);
		}
	}

	void beginPush()
	{
		pushing = true;
	}
	
	void endPush()
	{
		pushing = false;
	}
	
	Vector3 getForceVector()
	{
		oldDirection = direction;
		oldMagnitude = magnitude;
		
		GameObject temp = new GameObject();
		temp.transform.RotateAround(Vector3.zero, -Vector3.forward, direction);
		temp.transform.RotateAround(Vector3.zero, Vector3.forward, transform.eulerAngles.z);
		Vector3 result = temp.transform.up * magnitude;
		GameObject.Destroy(temp);
		return result;
	}
}
