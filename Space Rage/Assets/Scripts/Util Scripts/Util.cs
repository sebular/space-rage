using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {
    private static bool playerListNeedsUpdate = true;
    private static GameObject[] playerList;
    private static Util utilInstance;
    private static int lives = 5;
    private static int[] playerLives = new int[] {lives, lives, 0, 0};
    private static bool gameOver = false;

	// Use this for initialization
	void Start () {
	
	}

    void OnGUI()
    {
        //GUI.Box(new Rect(30, 30, 140, 23), "Player 1 deaths: " + playerDeathCount[0].ToString());
        //GUI.Box(new Rect(Screen.width - 170, 30, 140, 23), "Player 2 deaths: " + playerDeathCount[1].ToString());

        //GUI.Box(new Rect(30, 60, 140, 23), "Player 1 wins: " + playerWinCount[0].ToString());
        //GUI.Box(new Rect(Screen.width - 170, 60, 140, 23), "Player 2 wins: " + playerWinCount[1].ToString());
    }

    void Update()
    {
        if (Input.GetButtonUp("P1 Start") || Input.GetButtonUp("P2 Start"))
        {
            if (gameOver) {
                Time.timeScale = 1;
                gameOver = false;
                Application.LoadLevel(Application.loadedLevel);   
            }
            else 
            {
                if (Time.timeScale == 0) {
                    Time.timeScale = 1;
                }
                else {
                    Time.timeScale = 0;
                }
            }
        }
    }

    public static Util utilObject()
    {
        if (utilInstance == null) utilInstance = (Util)Camera.main.GetComponent("Util");
        return utilInstance;
    }

    public static GameObject[] getPlayers()
    {
        if (Util.playerListNeedsUpdate)
        {
            Util.playerList = GameObject.FindGameObjectsWithTag("Player");
            Util.playerListNeedsUpdate = false;
        }
        return Util.playerList;
    }

    public static int getPlayerLives(int player)
    {
        return playerLives[player];
    }

    public static void setPlayerListNeedsUpdate()
    {
        Util.playerListNeedsUpdate = true;
    }

    public static void Respawn(int playerNumber, Vector3 startingPosition, Quaternion startingRotation)
    {
        playerLives[playerNumber - 1]--;
        checkForWin();

        if (!gameOver)
        {
            Util u = utilObject();
            Time.timeScale = .35f;
            u.StartCoroutine(u.delayedRespawn(playerNumber, startingPosition, startingRotation));
        }
        else
        {

        }
    }

    IEnumerator delayedRespawn(int playerNumber, Vector3 startingPosition, Quaternion startingRotation)
    {
        yield return new WaitForSeconds(2.0f);
        Time.timeScale = 1;
        Vector3 newSpawn = Util.generateSpawnPoint();
        GameObject newPlayer = (GameObject)Instantiate(Resources.Load("Player"), newSpawn, startingRotation);
        newPlayer.name = "Player";
        newPlayer.SendMessage("setPlayerNumber", playerNumber);
        Util.setPlayerListNeedsUpdate();
        this.SendMessage("updatePlayers");
    }

    public static Vector3 generateSpawnPoint()
    {
        Vector3 point = Util.generatePotentialSpawnPoint();
        while (Physics2D.OverlapCircleAll(point, 10f).Length > 1)
        {
            point = Util.generatePotentialSpawnPoint();
        }
        return point;
    }

    private static Vector3 generatePotentialSpawnPoint()
    {
        float x = ((float)Random.Range(0, 98) + 1) / 100f;
        float y = ((float)Random.Range(0, 98) + 1) / 100f;
        Vector3 point = new Vector3(x, y, 0);
        point = Camera.main.ViewportToWorldPoint(point);
        point.z = 0;
        return point;
    }

    static bool checkForWin()
    {
        int total = playerLives[0] + playerLives[1]; //+ playerLives[2] + playerLives[3];
        if (playerLives[0] == 0)
        {
            Debug.Log("Player 1 loses!");
            GameObject playerScore = GameObject.Find("Player Score 1");
            playerScore.SendMessage("lose");
        }
        if (playerLives[1] == 0)
        {
            Debug.Log("Player 2 loses!");
            GameObject playerScore = GameObject.Find("Player Score 2");
            playerScore.SendMessage("lose");
        }

        if (playerLives[0] == total)
        {
            Debug.Log("Player 1 wins!");
            playerLives[0] = lives;
            playerLives[1] = lives;
            Time.timeScale = 0.05f;
            GameObject playerScore = GameObject.Find("Player Score 1");
            playerScore.SendMessage("win");
            gameOver = true;
            return true;
        }
        if (playerLives[1] == total)
        {
            Debug.Log("Player 2 wins!");
            playerLives[0] = lives;
            playerLives[1] = lives;
            Time.timeScale = 0.05f;
            GameObject playerScore = GameObject.Find("Player Score 2");
            playerScore.SendMessage("win");
            gameOver = true;
            return true;
        }
        return false;
    }
}
