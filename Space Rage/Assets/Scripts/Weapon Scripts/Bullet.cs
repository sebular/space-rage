using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    GameObject parentPlayer;

	// Use this for initialization
	void Start () {
		//transform.localScale = new Vector3(xScale, 0, zScale);
        rigidbody2D.velocity = transform.up * 100;
        Destroy(gameObject, 2);
		//Time.timeScale = .05f;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (transform.localScale.y < length) {
			transform.localScale = Vector3.Slerp(transform.localScale, new Vector3 (xScale, length, zScale), Time.deltaTime * 30);
		} 
		else if (transform.localScale.y > length) {
			transform.localScale = new Vector3 (xScale, length, zScale);
		}
		*/
	}

    void OnCollisionEnter2D()
    {
        Destroy(gameObject);
        //Time.timeScale = .3f;
    }

    void OnDestroy()
    {
        //Time.timeScale = 1;
    }

    public void setCharge(float charge)
    {
        transform.localScale *= charge;
        transform.position += transform.up * (charge * .5f);
		rigidbody2D.mass = charge;
		Color c = renderer.material.color;
		c.r = 1 - (charge - 1) * .1f;
		c.g = 1 - (charge - 1) * .1f;
		renderer.material.color = c;
    }
}
 