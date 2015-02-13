using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class WallBuilder : MonoBehaviour {
    LevelController levelController;
	bool editing = false;
	bool draggingSlider = false;
	Vector3 targetScale;
	Vector3 targetPosition;

	
	GameObject sliderBar;
    Draggable sliderScript;

    Block currentBlock;

    Color playColor;
    Color editColor;

	// Use this for initialization
	void Start () {
		
        sliderBar = GameObject.Find("Draggable");
        sliderScript = (Draggable)GameObject.Find("Draggable").transform.FindChild("Slider").gameObject.GetComponent("Draggable");
        levelController = new LevelController(new Level(), transform.FindChild("Wall").gameObject, transform.FindChild("Goal").gameObject, transform.FindChild("Force").gameObject, transform.FindChild("Planet").gameObject, transform.FindChild("Alt").gameObject);
		targetScale = levelController.getCursor().localScale / 4;
    	playColor = Camera.main.backgroundColor;
        //editColor = new Color32(232, 162, 162, 255);s
		editColor = Camera.main.backgroundColor;
        levelController.load();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.E)) {
			editing = !editing;
			if (editing) startEditing();
			else stopEditing();
		}
		if (editing) {
			controlCamera();   
		}
        
	}

    void LateUpdate()
    {
        if (editing)
        {
            edit();
            positionPalette();
        }
    }

	void handleInput() {
		if (Input.GetKeyUp(KeyCode.P)) snapshot();
        if (Input.GetKeyUp(KeyCode.V)) levelController.save();
        if (Input.GetKeyUp(KeyCode.L)) levelController.load();
		if (Input.GetKeyUp(KeyCode.Equals)) levelController.increaseBlockSize();
		if (Input.GetKeyUp(KeyCode.Minus)) levelController.decreaseBlockSize();
		if (Input.GetKeyUp(KeyCode.R)) rotateBlock();
		if (Input.GetKeyUp(KeyCode.Tab)) {
			draggingSlider = !draggingSlider;
			if (draggingSlider)
			{
				sliderBar.transform.position = new Vector3(targetPosition.x, targetPosition.y + 100, targetPosition.z);
				currentBlock = levelController.getBlock(levelController.getCursor().position);
				Debug.Log(currentBlock.type);
				
			}
			else
			{
				currentBlock = null;
				sliderBar.transform.position = new Vector3(float.MaxValue, float.MaxValue, sliderBar.transform.position.z);
			}
		}
		if (draggingSlider) {
			drag();
		}
	}

	void edit() {
		handleInput();
		SetTargetPosition();
		if (!palette()) {
			if (Input.GetButton("Fire1"))
			{
				//AddBlockAtTarget();
                levelController.addBlockAtCursor();
			}
			else if (Input.GetButton("Fire2"))
			{
				//RemoveBlockAtTarget();
                levelController.removeBlockAtCursor();
			}
		}
	}

	void startEditing() {
        levelController.enableCursor();
		Camera.main.backgroundColor = editColor;
		CameraBehavior camBehavior = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
		camBehavior.enabled = false;
		PlayerControl playerControl = (PlayerControl)GameObject.Find("Player").GetComponent("PlayerControl");
		playerControl.enabled = false;
	}
	
	void stopEditing() {
        levelController.disableCursor();
		Camera.main.backgroundColor = playColor;
		CameraBehavior camBehavior = (CameraBehavior)Camera.main.GetComponent("CameraBehavior");
		camBehavior.enabled = true;
		PlayerControl playerControl = (PlayerControl)GameObject.Find("Player").GetComponent("PlayerControl");
		playerControl.enabled = true;
		transform.position = new Vector3(float.MaxValue, float.MaxValue, transform.position.z);
	}

	

  	void drag() {
        if (currentBlock != null)
        {
            currentBlock.strength = sliderScript.percentage;
        }
  	}

	void saveBlockAttributes() {

	}

	bool palette() {
		if (Input.GetButton("Fire1")) {
			Vector3 position = levelController.getCursor().position;
			Collider[] colliders;
			if ((colliders = Physics.OverlapSphere(position, 1f)).Length > 1) 
			{
				foreach (var collider in colliders) {
					var go = collider.gameObject;
					if (go.name == "Background") return true;
				}
			}
		}

		if (Input.GetButtonUp("Fire1")) {
            Vector3 position = levelController.getCursor().position;

			Collider[] colliders;
			if ((colliders = Physics.OverlapSphere(position, 1f)).Length > 1)
			{
				bool onPalette = false;
				bool targetClicked = false;
				bool targetSet = false;
				GameObject newTarget = colliders[0].gameObject;

				foreach (var collider in colliders)
				{
					GameObject go = collider.gameObject;
					if (go == levelController.getCursorGameObject()) continue;
					if (go.name == "Background") onPalette = true;
					if (levelController.isObstacle(go)) {
						targetClicked = true;
						if (!targetSet) {
							newTarget = go;
							targetSet = true;
						}
					}
					if (onPalette && targetClicked) continue;
				}
				if (onPalette && targetClicked) {
					changeTarget(newTarget);
					return true;
				}
				if (onPalette) {
					return true;
				}
			}
		}
		return false;
	}

	void changeTarget(GameObject newTarget) {
        levelController.setCursorParent(transform);
		newTarget.transform.parent = null;
        levelController.setCursor(newTarget);
	}

	void zoomOutView() {
		Camera.main.orthographicSize = 1200;
	}

	void zoomInView() {
		Camera.main.orthographicSize = 400;
	}
	
	void SetTargetPosition() {
		targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 originalPosition = targetPosition;
		targetPosition.z = 0;
		targetPosition.x = (int)(targetPosition.x / targetScale.x) * targetScale.x;
		targetPosition.y = (int)(targetPosition.y / targetScale.y) * targetScale.y;
		targetPosition.x = (originalPosition.x > 0) ? targetPosition.x + (targetScale.x / 2) : targetPosition.x - (targetScale.x / 2);
		targetPosition.y = (originalPosition.y > 0) ? targetPosition.y + (targetScale.y / 2) : targetPosition.y - (targetScale.y / 2);
        levelController.setCursorPosition(targetPosition);
        levelController.setAlternateCursorPosition(targetPosition);
	}

	
	
	void snapshot() {
		Application.CaptureScreenshot(Path.Combine(Application.dataPath, "Previews/Resources/screen-" + Application.loadedLevel + ".png"));
		Debug.Log ("snapshot taken: screen-" + Application.loadedLevel + ".png");
	}
	

	
	void rotateBlock()
	{
		Vector3 currentRotation = levelController.getCursor().rotation.eulerAngles;
		currentRotation.z += 90;
        levelController.rotateCursor(currentRotation);
	}

    void controlCamera()
    {
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * 1000,
            Camera.main.transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * 1000,
            Camera.main.transform.position.z);
        if (Application.platform.ToString() == "Android")
        {
            Camera.main.orthographicSize -= Input.GetAxis("OUYA L2") * Time.deltaTime * 1000;
            Camera.main.orthographicSize += Input.GetAxis("OUYA R2") * Time.deltaTime * 1000;
        }
        else
        {
            Camera.main.orthographicSize += Input.GetAxis("Triggers") * Time.deltaTime * 1000;
            Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 3000;
        }
    }

    void positionPalette()
    {
        Vector3 paletteScale = transform.Find("Background").localScale;

        Vector3 paletteLeftBorder = new Vector3(transform.position.x - (paletteScale.x / 2), transform.position.y, transform.position.z);
        Vector3 paletteTopBorder = new Vector3(transform.position.x, transform.position.y + (paletteScale.y / 2), transform.position.z);

        Vector3 paletteLeftScreen = Camera.main.WorldToScreenPoint(paletteLeftBorder);
        Vector3 paletteTopScreen = Camera.main.WorldToScreenPoint(paletteTopBorder);
        Vector3 paletteCenterScreen = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 palettePosition = Camera.main.ScreenToWorldPoint(new Vector3(paletteCenterScreen.x - paletteLeftScreen.x, (paletteTopScreen.y - paletteCenterScreen.y), 0));
        palettePosition.z = transform.position.z;
        transform.position = palettePosition;
    }
}
