using UnityEngine;
using System.Collections;

public class ForceArea : MonoBehaviour {
    public float direction = 0;
    public float magnitude = 100;
    
    GameObject player;
    bool pushing = false;
    Vector3 forceVector;
    float oldDirection = 0;
    float oldMagnitude = 100;
	
    Transform slideUp;
    Transform slideDown;
    Transform slideLeft;
    Transform slideRight;
    Transform slider;
    bool sliding = false;

	// Use this for initialization
	void Start () {
	    player = GameObject.Find("Player");
        forceVector = getForceVector();
        Quaternion rot = Quaternion.Euler(0, 0, direction);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, new Vector3(1, 1, 1));

		slideUp = transform.Find("SlideUp");
        //slideDown = transform.Find("SlideDown");
        //slideLeft = transform.Find("SlideLeft");
        //slideRight = transform.Find("SlideRight");

        if (direction == 0)
        {
            slider = transform.Find("SlideUp");
        }
        else if (direction == 90)
            slider = transform.Find("SlideLeft");
        else if (direction == 180)
            slider = transform.Find("SlideDown");
        else if (direction == 270)
            slider = transform.Find("SlideRight");
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!player) {
			if(pushing) endPush();
			//player = GameObject.Find("Player");
		}

		if (pushing) {
			Push(player);
		}
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
        if (slider)
        {
            Slider s = slider.GetComponent<Slider>();
            s.moving = true;
        }
    }

    void endPush()
    {
        pushing = false;
        if (slider)
        {
            Slider s = slider.GetComponent<Slider>();
            s.moving = false;
        }
    }
        
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player = other.gameObject;
            beginPush();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            endPush();
        }
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
