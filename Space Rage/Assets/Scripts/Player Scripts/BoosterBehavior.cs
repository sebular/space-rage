using UnityEngine;
using System.Collections;

public class BoosterBehavior : MonoBehaviour {
	bool exploded = false;
    bool boostingSound = false;
    bool boostFadingIn = false;
    bool boostFadingOut = false;

	void Start () {
	
	}
	
	void FixedUpdate () {
		rigidbody2D.AddForce(transform.up * Time.deltaTime * 3000);
	}

    void Update()
    {
        updateSound();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!exploded && other.gameObject.tag != "Piece" && other.gameObject.tag != "Player")
        {
            explode();
        }
    }

    void explode()
    {
		exploded = true;
        gameObject.rigidbody2D.velocity = Vector2.zero;
        

        gameObject.particleSystem.enableEmission = true;
        gameObject.particleSystem.Play();

        renderer.enabled = false;
        collider2D.enabled = false;

        transform.FindChild("Jet").gameObject.particleSystem.enableEmission = false;
		gameObject.rigidbody2D.isKinematic = true;
        Destroy(gameObject, 1);
    }






    // ----------------- Sound Stuff







    void updateSound() 
    {
        if (exploded) stopBoostSound();
    }

    void playBoostSound()
    {
        if (!boostingSound)
        {
            boostingSound = true;
            //boostFadingIn = true;
            //boostFadingOut = false;
            audio.volume = 1;
        }
    }

    void stopBoostSound()
    {
        if (audio.volume > 0.01) audio.volume -= 5 * Time.deltaTime;
    }
}
