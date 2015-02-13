using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool canResume = false;
	
	float screenWidth, screenHeight, itemWidth, itemHeight, buttonWidth, buttonHeight, spacing;
	int buttonPadding;
	int selectedIndex = 0;
	int lastIndex = 2;
	public GUIStyle style;
	Vector2 scrollPosition = Vector2.zero;
	string[] selStrings = new string[] { "Resume", "Select Level" };
	float TIME_DELAY = .12f;
	float delayCount = 0;
	
	void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        itemWidth = screenWidth / 5;
        itemHeight = itemWidth * .6f;
		spacing = screenWidth / 8;
		buttonPadding = 0;
		buttonWidth = itemWidth - buttonPadding * 2;
		buttonHeight = (itemHeight) - buttonPadding * 2;
		delayCount = TIME_DELAY;
	}
	
	// Update is called once per frame
	void Update () {
		keyboardFunctionality();
		if( canResume && Input.GetButtonUp("Fire3") ) {
			Debug.Log("RESUME");
			PauseMenu menu = (PauseMenu)Camera.main.GetComponent("PauseMenu");
			CameraBehavior cameraScript = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
			Time.timeScale = 1;
			cameraScript.canPause = false;
			cameraScript.enabled = true;
            menu.enabled = false;
		}
		if (!canResume) canResume = true;
	}

    void OnGUI()
    {
		int tempSelectedIndex = GUI.SelectionGrid(new Rect(spacing, spacing * 2, screenWidth - spacing * 2, screenHeight - spacing * 3), selectedIndex, selStrings, 2, style);
    }
	
	void keyboardFunctionality() {
	    if (Input.GetAxisRaw("P1 Horizontal") < 0 || Input.GetAxisRaw("P2 Horizontal") < 0)
	    {
			delayCount = 0;
		    if( selectedIndex > 0 )
				selectedIndex--;
		}

        if (Input.GetAxisRaw("P1 Horizontal") > 0 || Input.GetAxisRaw("P2 Horizontal") > 0)
		{
			delayCount = 0;
		    if( selectedIndex + 1 < lastIndex )
		        selectedIndex++;
		}
			
		//Debug.Log(selectedIndex);
		
	    if( Input.GetButton("P1 Fire1") || Input.GetButton("P2 Fire1") || Input.GetButton("Jump") || Input.GetKeyUp(KeyCode.Return) ) {
			if (selectedIndex == 0) {
	        	Debug.Log("RESUME");
				PauseMenu menu = (PauseMenu)Camera.main.GetComponent("PauseMenu");
				CameraBehavior cameraScript = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
				Time.timeScale = 1;
				cameraScript.canPause = false;
				cameraScript.enabled = true;
            	menu.enabled = false;
			}
			if (selectedIndex == 1) {
				Application.LoadLevel(Application.levelCount - 1);
            	Time.timeScale = 1;
			}
		}
	}
}
