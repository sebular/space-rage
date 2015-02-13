using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public float rotationSpeed = 200.0F;
    public float initialDrag = 0F;
	public float jetFuel = 5;
    public int playerNumber = 1;
    public int jetPower = 3000;
	public bool isKeyboard = true;
    
    
    Transform leftJet;
    Transform rightJet;
    BoosterBehavior leftBooster;
    BoosterBehavior rightBooster;

    Vector2 shipVelocity;
    Vector3 startingPosition;
    Quaternion startingRotation;
	
	float fuel;
    float debrisLifetime = 15;

    bool dead = false;   // keep a flag to make sure the collision event doesn't trigger a double respawn

    float missilesLeft = 0;
    float bulletCharge = 1;
    float maxBulletCharge = 5;
	Transform chargeOrb;

    float health = 100;
    string playerNumberString;

    bool dashing = false;
    bool boostingUp = false;
    
    bool boostingSound = false;
    bool boostFadingIn = false;
    bool boostFadingOut = false;

    float dashLength = .2f;
    float dashDelay = 1;

    GameObject[] pieces;
    GameObject healthBar;

	void Start () {
        initialize();
	}
	
	void FixedUpdate () {
        if (!dead)
        {
            shipVelocity = rigidbody2D.velocity;
            handleInputFixed();
        }
	}

    void Update()
    {
        if (!dead)
        {
            handleInput();
            updateHealth();
        }
        updateSound();
    }

    void resetPosition() 
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;        
    }
	
	void resetFuel() {
		fuel = jetFuel;
	}






    // ----------------------- Handle Input






    void handleInputFixed()
    {

		rotate();
        boost();
    }

    void handleInput()
    {
        if (Input.GetButtonDown(controllerString("Shoot")))
        {
            fireWeapon("missile");
        }

        if (Input.GetButton(controllerString("LB")))
        {
            chargeBullet();
        }
        if (Input.GetButtonUp(controllerString("LB")))
        {
            //if (!dashing) startDash();
            fireWeapon("bullet");
        }
        
    }

	string controllerString(string key) {
		return (isKeyboard) ? playerNumberString + key + " KB" : playerNumberString + key;
	}
    





    // ----------------------- Player Movement






    private void rotate()
    {
        float rotation = Input.GetAxis(controllerString("Horizontal")) * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, -rotation);
    }

    private void boost()
    {
        
        if (fuel > 0)
        {
            if (boostUp())
            {
                burnFuel();
            }
        }
        else
        {
            leftJet.particleSystem.enableEmission = false;
            rightJet.particleSystem.enableEmission = false;
        }
    }

    private bool boostUp()
    {
        boostingUp = Input.GetButton(controllerString("Boost")) || dashing;
        leftJet.particleSystem.enableEmission = boostingUp;
        rightJet.particleSystem.enableEmission = boostingUp;
        float boostPower = (dashing) ? jetPower * 10 : jetPower;
        if (boostingUp)
        {
            rigidbody2D.AddForce(transform.up * boostPower * Time.deltaTime);
        }
        return boostingUp;
    }

    private void burnFuel()
    {
        // For now let's just have infinite fuel.  Maybe later that won't be the case.
        //fuel -= Time.deltaTime; if (fuel < 0) fuel = 0;
        //float fuelPercentage = fuel / jetFuel;
    }





    // ----------------------- Weapons stuff






    private void fireWeapon(string weapon)
    {
        if (weapon == "missile")    // fire ze missiles.  Turn into classes if this gets big.
        {
            if (missilesLeft > 0)
            {
                launchMissile();
                /*
                if (!playerMissile) launchMissile();
                else
                {
                    Missile playerMissileScript = (Missile)playerMissile.GetComponent("Missile");
                    if (playerMissileScript.exploded) launchMissile();
                }
                 * */
            }
        }
        else if (weapon == "bullet")
        {
            fireBullet();
        }
    }

    private void launchMissile()
    {        
        GameObject playerMissile = (GameObject)Instantiate(Resources.Load("HomingMissile"), transform.position + (transform.up * 4f), transform.rotation);
        playerMissile.name = "missile";
        playerMissile.SendMessage("setParentPlayer", this.gameObject);
        missilesLeft--;
        updateMissileDisplay();
    }

    private void fireBullet()
    {
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Bullet"), transform.position + (transform.up * 4f), transform.rotation);
        float charge = Mathf.Min(bulletCharge, maxBulletCharge);
        bullet.SendMessage("setCharge", charge);
        bullet.name = "bullet";
        bulletCharge = 1;
		chargeOrb.localScale = Vector3.zero;
    }

    private void chargeBullet()
    {
        bulletCharge += Time.deltaTime;
		bulletCharge = Mathf.Min (5, bulletCharge);
		float chargeScale = (bulletCharge - 1) * .5f;
		chargeOrb.localScale = new Vector3 (chargeScale, .25f, chargeScale);
		//chargeOrb.localPosition = Vector3.Slerp(chargeOrb.localPosition, new Vector3(0, 2.5f + (chargeScale / 2f), 0), Time.deltaTime);
		if (bulletCharge <= 5) {
			Color c = chargeOrb.renderer.material.color;
			c.r = 1 - (chargeScale * .1f);
			c.g = 1 - (chargeScale * .1f);
			chargeOrb.renderer.material.color = c;
		}
    }


    private void addMissile()
    {
        if (missilesLeft >= 2) return;

        missilesLeft++;
        updateMissileDisplay();
    }

    private void updateMissileDisplay()
    {
        if (missilesLeft == 0)
        {
            transform.FindChild("Missiles").FindChild("left").renderer.enabled = false;
            transform.FindChild("Missiles").FindChild("right").renderer.enabled = false;
        }
        if (missilesLeft == 1)
        {
            transform.FindChild("Missiles").FindChild("left").renderer.enabled = true;
            transform.FindChild("Missiles").FindChild("right").renderer.enabled = false;
        }
        else if (missilesLeft == 2)
        {
            transform.FindChild("Missiles").FindChild("left").renderer.enabled = true;
            transform.FindChild("Missiles").FindChild("right").renderer.enabled = true;
        }
    }




    // ------------------------Item Handling






    public void pickUpItem(string item)
    {
        switch (item)
        {
            case "missile":
                addMissile();
                break;
            default:
                break;
        }
    }





    // -----------------Collision, Damage, Death






    void OnCollisionEnter2D(Collision2D collision)
    {
        takeDamage(collision);
    }

    void takeDamage(Collision2D collision)
    {
        float damageTaken = 0;
        float hitSpeed = collision.relativeVelocity.magnitude;
        switch (collision.gameObject.name)
        {
            // "missile", "asteroid", "bullet", "Player"
            case "missile":
                damageTaken = 50;
                if (hitSpeed > 50) damageTaken = hitSpeed;
                break;
            case "asteroid":
                damageTaken = hitSpeed * .3f;
                break;
            case "bullet":
                damageTaken = 17 * collision.rigidbody.mass;
                break;
            case "debris":
                damageTaken = hitSpeed * .1f;
                break;
            default:
                break;
        }
        health -= damageTaken;

        if (health < 0)
        {
            tryDie(collision);
        }
    }

    void updateHealth()
    {
        if (health < 100)
        {
            health += 15 * Time.deltaTime;
        }
        healthBar.SendMessage("setHealth", health);
    }

    void tryDie(Collision2D collision)
    {
        if (!dead)
        {
            dead = true;
            Break(collision, boostingUp);
            Destroy(this.gameObject, .5f);
            healthBar.SendMessage("setHealth", 0f);
            Util.Respawn(playerNumber, startingPosition, startingRotation);
        }
    }

    void Break(Collision2D collision, bool boosting)
    {
        // Make sure the player body doesn't interfere with the pieces
        collider2D.enabled = false;
        collider2D.isTrigger = true;

        foreach (GameObject piece in pieces)
        {
            Rigidbody2D r = piece.AddComponent<Rigidbody2D>();
            BoxCollider2D c = piece.AddComponent<BoxCollider2D>();
            piece.AddComponent<Wraparound>();
            r.interpolation = RigidbodyInterpolation2D.Extrapolate;
            r.gravityScale = 0;
            piece.renderer.enabled = true;
            piece.transform.parent = null;
            c.enabled = true;
            //if (shipVelocity.magnitude > piece.rigidbody2D.velocity.magnitude) piece.rigidbody2D.velocity = shipVelocity;
            r.velocity = shipVelocity;
            r.AddTorque(20);
            Destroy(piece.gameObject, Random.value + 1 * debrisLifetime);
        }
        leftBooster.enabled = boosting;
        rightBooster.enabled = boosting;
    }

    void findPlayerPieces()
    {
        Transform geometry = this.transform.Find("Geometry");
        pieces = new GameObject[geometry.childCount];
        for (int i = 0; i < geometry.childCount; i++) {
            pieces[i] = geometry.GetChild(i).gameObject;
        }
    }




    // ----------------- Shield stuff






    private void startDash()
    {
        dashing = true;
        Invoke("endDash", dashLength);
    }

    void endDash()
    {
        dashing = false;
    }

    private void dash()
    {

    }




    // ---------------- Sound Stuff







    void updateSound()
    {
        if (boostingUp) playBoostSound();
        else stopBoostSound();
    }

    void playBoostSound()
    {
        if (!boostingSound)
        {
            boostingSound = true;
            boostFadingIn = true;
            boostFadingOut = false;
        }
        if (boostingSound && boostFadingIn)
        {
            if (audio.volume < 1) audio.volume += 10 * Time.deltaTime;
            else boostFadingIn = false;
        }
    }

    void stopBoostSound()
    {
        if (boostingSound)
        {
            boostingSound = false;
            boostFadingIn = false;
            boostFadingOut = true;
        }

        if (!boostingSound && boostFadingOut)
        {
            if (audio.volume > 0) audio.volume -= 5 * Time.deltaTime;
            else boostFadingOut = false;
        }
    }






    // -------------- Multiplayer differences






    public void setPlayerNumber(int number)
    {
        this.playerNumber = number;
        if (number != 1) setPlayerColor();
        healthBar = GameObject.Find("Player Score " + playerNumber);
        healthBar.SendMessage("destroyOrb");
    }

    private void setPlayerColor()
    {
        Material m = (Material)Resources.Load("Player " + playerNumber, typeof(Material));
        Transform geometry = transform.Find("Geometry");
        foreach (Transform child in geometry)
        {
            child.renderer.material = m;
        }
    }





   
    // ---------------- methods called from other scripts via sendMessage()





    void beginDrag(float drag)
    {
        rigidbody.drag = drag;
    }

    void endDrag()
    {
        rigidbody.drag = initialDrag;
    }





    // ---------------- Init stays here so I don't have to look at it






    private void initialize()
    {
        playerNumberString = "P" + playerNumber + " ";
        if (playerNumber != 1) setPlayerColor();
        fuel = jetFuel;
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        leftJet = this.transform.Find("Geometry").Find("Left Booster").Find("Jet");
        leftBooster = (BoosterBehavior)this.transform.Find("Geometry").Find("Left Booster").gameObject.GetComponent("BoosterBehavior");

        rightJet = this.transform.Find("Geometry").Find("Right Booster").Find("Jet");
        rightBooster = (BoosterBehavior)this.transform.Find("Geometry").Find("Right Booster").gameObject.GetComponent("BoosterBehavior");

        audio.volume = 0;
        //healthBar = GameObject.Find("Health Bar Player " + playerNumber);
        healthBar = GameObject.Find("Player Score " + playerNumber);
		chargeOrb = transform.FindChild ("Bullet Charge");
        findPlayerPieces();
    }
}
