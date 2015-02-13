using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour {

    GameObject parentPlayer;
    public float speed = 30;
    bool exploded = false;

	// Use this for initialization
	void Start () {
        rigidbody2D.velocity = transform.up * speed;
        Destroy(gameObject, 5);
	}

    void FixedUpdate()
    {
        if (exploded) return;
        if (!parentPlayer)
        {
            Destroy(gameObject);
            return;
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;
        if (players.Length == 1 && players[0] == parentPlayer) return;

        int count = 0;
        
        for (int i = 0; i < players.Length; i++)
        {
            GameObject p = players[i];
            if (p != parentPlayer) count++;
            else players[i] = null;
        }

        GameObject[] targets = new GameObject[count];
        count = 0;
        for (int i = 0; i < players.Length; i++)
        {
            GameObject p = players[i];
            if (p != null)
            {
                targets[count] = p;
                count++;
            }
        }

        GameObject target = findClosestTarget(targets);

        if (target)
        {
            //Debug.DrawLine(transform.position, target.transform.position);
            float distance = (target) ? (transform.position - target.transform.position).magnitude : 0;
            if (target && target.rigidbody2D && distance < 40 && target.gameObject.name == "Player")
            {
                Vector2 targetDirection = (target.transform.position - transform.position).normalized;
                Vector2 lookDirection = Vector2.Lerp(rigidbody2D.velocity.normalized, targetDirection, Time.deltaTime * 5);
                //Vector3 front = new Vector3(lookDirection.x, lookDirection.y, 0) * 10;
                rigidbody2D.velocity = lookDirection * speed;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, -transform.forward);
                transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
        }
        else
            rigidbody2D.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.name == "asteroid")
        {
            gameObject.particleSystem.enableEmission = true;
            gameObject.particleSystem.Play();
            Destroy(gameObject, .3f);
            exploded = true;
            rigidbody2D.isKinematic = true;
        }
    }

    GameObject findClosestTarget(GameObject[] targets)
    {
        int closestIndex = -1;
        if (targets.Length == 1)
        {
            closestIndex = 0;
        }
        else
        {
            Vector3 distance = transform.position - targets[0].transform.position;

            for (int i = 1; i < targets.Length; i++)
            {
                Vector3 newDistance = transform.position - targets[i].transform.position;
                if (distance.magnitude > newDistance.magnitude)
                {
                    distance = newDistance;
                    closestIndex = i;
                }
            }
        }
        //Debug.Log((targets[0].transform.position - transform.position).magnitude);
        return ((targets[closestIndex].transform.position - transform.position).magnitude < 700) ? targets[closestIndex] : null;
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    public void setParentPlayer(GameObject player)
    {
        parentPlayer = player;
        Debug.Log("set parent.");
    }
}
