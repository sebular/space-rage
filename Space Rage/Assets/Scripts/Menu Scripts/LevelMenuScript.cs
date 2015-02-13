using UnityEngine;
using System.Collections;

public class LevelMenuScript : MonoBehaviour {

    float screenWidth, screenHeight, itemWidth, itemHeight, buttonWidth, buttonHeight, spacing;
	int buttonPadding;
	int selectedIndex = 0;
    int lastIndex;
    //Vector2 center;
	public GUIStyle style;
	public GUIContent content;
	Vector2 scrollPosition = Vector2.zero;
	string[] selStrings = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
	float TIME_DELAY = .12f;
	float delayCount = 0;
	
	bool keyboardOrJoystick = false;
	
	// Use this for initialization
	void Start () {
        lastIndex = selStrings.Length;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        itemWidth = screenWidth / 5;
        itemHeight = itemWidth * .6f;
		spacing = 80;
		buttonPadding = 0;
		buttonWidth = itemWidth - buttonPadding * 2;
		buttonHeight = (itemHeight) - buttonPadding * 2;
		delayCount = TIME_DELAY;
	}
	
	// Update is called once per frame
	void Update () {
		keyboardFunctionality();
	}

    void OnGUI()
    {
		//if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) selectedIndex = -1;
		int tempSelectedIndex = GUI.SelectionGrid(new Rect(spacing, spacing, screenWidth - spacing * 2, screenHeight - spacing * 2), selectedIndex, selStrings, 4, style);
		
		//if (selectedIndex != -1) Application.LoadLevel(selectedIndex + 2);
    }
	
	void keyboardFunctionality() {
		if ( delayCount == TIME_DELAY) {
			// Get keyboard input and increase or decrease our grid integer
		    if(Input.GetAxisRaw("Horizontal") < 0)
		    {
				delayCount = 0;
		        if( selectedIndex > 0 && selectedIndex % 4 != 0 )
					selectedIndex--;
		    }
		 
		    if(Input.GetAxisRaw("Horizontal") > 0)
		    {
				delayCount = 0;
		        if( selectedIndex + 1 < lastIndex && (selectedIndex + 1) % 4 != 0 )
		            selectedIndex++;
		    }
			if(Input.GetAxisRaw("Vertical") > 0) {
				delayCount = 0;
				if (selectedIndex - 4 >= 0)
					selectedIndex -= 4;
			}
			if(Input.GetAxisRaw("Vertical") < 0) {
				delayCount = 0;
				if (selectedIndex + 4 < lastIndex)
					selectedIndex += 4;
			}
			
			//Debug.Log(selectedIndex);
		}
		else {
			if (delayCount < TIME_DELAY)
				delayCount += Time.deltaTime;
			if (delayCount > TIME_DELAY) 
				delayCount = TIME_DELAY;
		}
		
	    if( Input.GetButton("Fire1") || Input.GetButton("Jump") || Input.GetKeyUp(KeyCode.Return) )
	        Application.LoadLevel(selectedIndex + 2);

        if (Input.GetButtonUp("Fire2"))
            Application.LoadLevel(0);
	}
}
