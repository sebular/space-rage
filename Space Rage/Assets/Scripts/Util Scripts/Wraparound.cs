using UnityEngine;
using System.Collections;

public class Wraparound : MonoBehaviour {

    Camera camera;
    int teleportCount = 0;
	
	void Start () {
        camera = Camera.main;
	}

	void Update () {
        
        Vector3 viewportPosition = camera.WorldToViewportPoint(transform.position);
        Vector3 oldPosition = viewportPosition;
        viewportPosition.x = Mathf.Repeat(viewportPosition.x, 1);
        viewportPosition.y = Mathf.Repeat(viewportPosition.y, 1);
        if (viewportPosition != oldPosition)
            transform.position = camera.ViewportToWorldPoint(viewportPosition);
	}
}
