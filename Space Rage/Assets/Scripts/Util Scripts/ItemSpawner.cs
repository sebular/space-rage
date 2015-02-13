using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {
    float timeUntilSpawn = 5;
    int itemLimit = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (timeUntilSpawn <= 0 && currentItemCount() < itemLimit)
        {
            timeUntilSpawn = Random.Range(7, 15);
            Vector3 spawnPoint = Util.generateSpawnPoint();
            GameObject item = (GameObject)Instantiate(Resources.Load("Item"), spawnPoint, Quaternion.identity);
            item.transform.parent = transform;
        }
        else if (currentItemCount() < itemLimit)
        {
            timeUntilSpawn -= Time.deltaTime;
        }
	}

    private int currentItemCount()
    {
        return transform.childCount;
    }
}
