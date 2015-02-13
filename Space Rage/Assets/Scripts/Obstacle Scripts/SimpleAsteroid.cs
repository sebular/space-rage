using UnityEngine;
using System.Collections;

public class SimpleAsteroid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float scale = Random.Range(5, 15);
        transform.localScale = new Vector3(scale, scale, .1f);
        transform.position = Util.generateSpawnPoint();
        transform.Rotate(Vector3.forward, Random.Range(0, 89));
        rigidbody2D.velocity = new Vector2(Random.Range(-12, 12), Random.Range(-12, 12));
        rigidbody2D.angularVelocity = Random.Range(-100, 100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
