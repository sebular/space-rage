using UnityEngine;
using System.Collections;

public class FancyWraparound : MonoBehaviour {
    Camera camera;
    Vector3 screenBottomLeft, screenTopRight;
    float screenWidth, screenHeight;
    Transform[] ghosts = new Transform[8];
    bool scaleSet = false;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        screenBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        screenTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;
        CreateGhostShips();
        PositionGhostShips();
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 viewportPosition = camera.WorldToViewportPoint(transform.position);
        Vector3 oldPosition = viewportPosition;
        viewportPosition.x = Mathf.Repeat(viewportPosition.x, 1);
        viewportPosition.y = Mathf.Repeat(viewportPosition.y, 1);
        
        if (viewportPosition != oldPosition)
        {
            transform.position = camera.ViewportToWorldPoint(viewportPosition);
        }

		PositionGhostShips ();
        if (!scaleSet)
        {
            for (int i = 0; i < 8; i++)
            {
                ghosts[i].transform.localScale = transform.localScale;
            }
            scaleSet = true;
        }
	}

    void CreateGhostShips()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = (Transform)Instantiate(transform, Vector3.zero, Quaternion.identity);
            ghosts[i].transform.parent = transform.parent;
            //ghosts[i].transform.localScale = transform.localScale;

            DestroyImmediate(ghosts[i].GetComponent<FancyWraparound>());
            //ghosts[i].GetComponent<PlayerControl>().enabled = false;
            DestroyImmediate(ghosts[i].GetComponent<SimpleAsteroid>());
            DestroyImmediate(ghosts[i].GetComponent<Rigidbody2D>());
            //ghosts[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void PositionGhostShips()
    {
        Vector3 ghostPosition = transform.position;
		
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

		for (int i = 0; i < 8; i++)
		{
			ghosts[i].rotation = transform.rotation;
		}
    }

    void SwapShips()
    {
        foreach (var ghost in ghosts)
        {
            if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
                ghost.position.y < screenHeight && ghost.position.y > -screenHeight)
            {
                transform.position = ghost.position;

                break;
            }
        }

        PositionGhostShips();
    }
}
