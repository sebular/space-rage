using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
    public bool canPause = false;

    private GameObject[] players;
	private float zPosition;
    private int velocityLimit = 350;
	
	// Use this for initialization
	void Start () {
        updatePlayers();
		zPosition = transform.position.z;
        velocityLimit = (int)Mathf.Min(camera.pixelWidth, camera.pixelHeight);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        updatePlayers();
        //cameraMoveTarget();
        //cameraZoom();
        checkForPause();
	}

    public void updatePlayers()
    {
        players = Util.getPlayers();
    }

    private void cameraZoom()
    {
        float playerDistance = maxPlayerDistance();
        float size = (playerDistance > 1000) ? 1000 : playerDistance;

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, size, Time.deltaTime);
    }

    private void cameraMoveTarget()
    {
        Vector3 target = averagePositions();
        if (double.IsNaN(target.x) || double.IsNaN(target.y) || double.IsNaN(target.z))
            target = transform.position;
        target.z = zPosition;
        transform.position = Vector3.Lerp(transform.position, target, 2f * Time.deltaTime);
    }

    private float maxPlayerDistance()
    {
        updatePlayers();
        if (players.Length < 2) return 0;
        if (players.Length == 2) return getPlayerDistance(0, 1);
        if (players.Length == 3)
        {
            float zeroOne = getPlayerDistance(0, 1);
            float zeroTwo = getPlayerDistance(0, 2);
            float oneTwo = getPlayerDistance(1, 2);
            float[] values = new float[] {zeroOne, zeroTwo, oneTwo};
            return Mathf.Max(values);
        }
        if (players.Length == 4)
        {
            float[] values = new float[] {
                getPlayerDistance(0, 1),
                getPlayerDistance(0, 2),
                getPlayerDistance(0, 3),
                getPlayerDistance(1, 2),
                getPlayerDistance(1, 3),
                getPlayerDistance(2, 3)
            };
            return Mathf.Max(values);
        }
        return 0;
    }

    private float getPlayerDistance(int one, int two)
    {
        updatePlayers();
        try
        {
            return (players[one].transform.position - players[two].transform.position).magnitude;
        }
        catch
        {
            return 0;
        }
    }

    private Vector3 averagePositions()
    {
        int totalPlayers = 0;
        Vector3 result = Vector3.zero;
        updatePlayers();
        foreach (GameObject p in players)
        {
            if (p != null)
            {
                result += p.transform.position;
                totalPlayers++;
            }
        }
        result /= totalPlayers;
        return result;
    }

    // Not currently being used
    private Vector3 getCameraLookAhead(GameObject player)
    {
        if (player.rigidbody.velocity.magnitude <= velocityLimit)
        {
            return player.rigidbody.velocity;
        }
        return player.rigidbody.velocity.normalized * velocityLimit;
    }

    private void checkForPause()
    {
        if (canPause && Input.GetButtonUp("Fire3"))
        {
            PauseMenu menu = (PauseMenu)Camera.main.GetComponent("PauseMenu");
            CameraBehavior cameraScript = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
            cameraScript.enabled = false;
            menu.enabled = true;
            menu.canResume = false;
            Time.timeScale = 0;
        }
        if (!canPause) canPause = true;
    }
}
