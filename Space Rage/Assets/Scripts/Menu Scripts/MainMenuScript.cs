using UnityEngine;using System.Collections;public class MainMenuScript : MonoBehaviour {    public float screenWidth, screenHeight, itemWidth, itemHeight, buttonWidth, buttonHeight, spacing;	int buttonPadding;	int selectedIndex = 0;	int lastIndex = 2;	public GUIStyle style;	Vector2 scrollPosition = Vector2.zero;	string[] selStrings = new string[] { /*Application.platform.ToString()*/ "Play", "Quit" };	float TIME_DELAY = .12f;	float delayCount = 0;		bool keyboardOrJoystick = false;		// Use this for initialization	void Start () {        screenWidth = Screen.width;        screenHeight = Screen.height;        itemWidth = screenWidth / 5;        itemHeight = itemWidth * .6f;		spacing = screenWidth / 8;		buttonPadding = 0;		buttonWidth = itemWidth - buttonPadding * 2;		buttonHeight = (itemHeight) - buttonPadding * 2;		delayCount = TIME_DELAY;		GameObject.Find("Left Booster").transform.Find("Jet").gameObject.particleSystem.enableEmission = true;        GameObject.Find("Right Booster").transform.Find("Jet").gameObject.particleSystem.enableEmission = true;	}		// Update is called once per frame	void Update () {		keyboardFunctionality();	}    void OnGUI()    {		int tempSelectedIndex = GUI.SelectionGrid(new Rect(spacing, spacing * 2, screenWidth - spacing * 2, screenHeight - spacing * 3), selectedIndex, selStrings, 2, style);    }		void keyboardFunctionality() {		if ( delayCount == TIME_DELAY) {			// Get keyboard input and increase or decrease our grid integer		    if(Input.GetAxisRaw("P1 Horizontal") < 0)		    {				delayCount = 0;		        if( selectedIndex > 0 )					selectedIndex--;		    }		 		    if(Input.GetAxisRaw("P1 Horizontal") > 0)		    {				delayCount = 0;		        if( selectedIndex + 1 < lastIndex )		            selectedIndex++;		    }						//Debug.Log(selectedIndex);		}		else {			if (delayCount < TIME_DELAY)				delayCount += Time.deltaTime;			if (delayCount > TIME_DELAY) 				delayCount = TIME_DELAY;		}			    if( Input.GetButton("P1 Fire1") || Input.GetButton("Jump") || Input.GetKeyUp(KeyCode.Return) ) {			if (selectedIndex == 0)                Application.LoadLevel(Application.levelCount - 1);			if (selectedIndex == 1)				Application.Quit();		}	}}