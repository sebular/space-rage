using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    GameObject parentPlayer;
    public bool exploded = false;
	// Use this for initialization
	void Start () {
        //transform.FindChild("Jet").gameObject.particleSystem.enableEmission = true;
        rigidbody.velocity = transform.up * 2000;
        Destroy(gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
        if (!rigidbody.isKinematic) rigidbody.velocity = transform.up * 2000;
	}

    void OnCollisionEnter(Collision other)
    {
        if (!exploded && other.gameObject && !other.gameObject.Equals(parentPlayer) && other.gameObject.tag != "Piece")
        {
            explode(other);
        }
    }

    public void setParentPlayer(GameObject player)
    {
        parentPlayer = player;
        
    }

    void explode(Collision other)
    {
        exploded = true;
        gameObject.particleSystem.enableEmission = true;
        gameObject.particleSystem.Play();
        renderer.enabled = false;
        transform.FindChild("Jet").gameObject.particleSystem.enableEmission = false;
        Destroy(gameObject, 1);
        Invoke("freeze", .05f);
    }

    void freeze()
    {
        gameObject.rigidbody.isKinematic = true;
        collider.enabled = false;
    }


    void splashDamage()
    {
        float radius = 50;
        float power = 10000;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            if (hit && hit.rigidbody && !hit.rigidbody.isKinematic)
                hit.rigidbody.AddExplosionForce(power, transform.position, radius);
        }
    }
}
