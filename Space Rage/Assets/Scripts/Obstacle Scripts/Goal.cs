using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public float MAX_ALLOWED_VELOCITY = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.gameObject.name == "Player" && collision.relativeVelocity.magnitude <= MAX_ALLOWED_VELOCITY)
        {            
            EndLevelMenu menu = (EndLevelMenu)Camera.main.GetComponent("EndLevelMenu");
			CameraBehavior cameraScript = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
			cameraScript.enabled = false;
            menu.enabled = true;
            PlayerPrefs.SetInt("Level " + Application.loadedLevel, 1);
        }
    }
}
