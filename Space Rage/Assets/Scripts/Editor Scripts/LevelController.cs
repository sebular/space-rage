using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class LevelController : MonoBehaviour {
    Level level;
    
    GameObject cursor;
    GameObject altCursor;
    GameObject wall;
    GameObject goal;
    GameObject force;
    GameObject planet;

    string[] blockNames = new string[] { "Wall", "Goal", "Force", "Planet" };

    public LevelController(Level level, GameObject wall, GameObject goal, GameObject force, GameObject planet, GameObject altCursor)
    {
        // Set arguments as local variables
        this.level = level;
        this.wall = wall;
        this.cursor = wall;
        this.goal = goal;
        this.force = force;
        this.planet = planet;
        this.altCursor = altCursor;

        // Initiate the cursor Game Object
        cursor.transform.parent = null;
        cursor.renderer.enabled = false;
    }

    public Block getBlock(Vector3 position)
    {
        return level.getBlock(position);
    }

    public void enableCursor()
    {
        cursor.renderer.enabled = true;
        cursor.collider.enabled = true;
    }

    public void disableCursor()
    {
        cursor.renderer.enabled = false;
        cursor.collider.enabled = false;
        altCursor.renderer.enabled = false;
    }

    public void setCursor(GameObject cursor)
    {
        this.cursor = cursor;
    }

    public void setCursorPosition(Vector3 position)
    {
        cursor.transform.position = position;
    }

    public void setCursorParent(Transform parent)
    {
        cursor.transform.parent = parent;
    }

    public void rotateCursor(Vector3 rotationAngles)
    {
        cursor.transform.Rotate(rotationAngles);
    }

    public Transform getCursor()
    {
        return cursor.transform;
    }

    public GameObject getCursorGameObject()
    {
        return cursor;
    }

    public void setAlternateCursorPosition(Vector3 targetPosition)
    {
        Vector3 altPosition = targetPosition;
        altPosition.z = altCursor.transform.position.z;
        altCursor.transform.position = altPosition;
        altCursor.renderer.enabled = level.HasBlockAtTarget((BlockType)Enum.Parse(typeof(BlockType), cursor.name), targetPosition);
    }

    public void addBlockAtCursor()
    {
        Vector3 position = cursor.transform.position;
        BlockType type = (BlockType)Enum.Parse(typeof(BlockType), cursor.name);
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(position, (cursor.transform.localScale.x - 1) / 2 /* Radius */)).Length < 4) //Presuming the object you are testing also has a collider 0 otherwise
        {
            if (level.AddBlockAtTarget(type, position, cursor.transform.localScale.x))
            {
                GameObject newBlock = (GameObject)Instantiate(cursor, cursor.transform.position, cursor.transform.rotation);
                newBlock.name = cursor.name;

                // enable planet gravity here, because we don't want the palette one affecting the ship
                if (newBlock.name == "Planet")
                {
                    Planet planet = (Planet)newBlock.GetComponent("Planet");
                    planet.enabled = true;
                }
            }
        }
    }

    public void removeBlockAtCursor()
    {
        Vector3 position = cursor.transform.position;
        BlockType type = (BlockType)Enum.Parse(typeof(BlockType), cursor.name);
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(position, (cursor.transform.localScale.x - 1) / 2 /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
        {
            string names = "";
            foreach (var collider in colliders)
            {
                GameObject go = collider.gameObject; //This is the game object you collided with
                names += go.name + " ";
                if (go == cursor) continue; //Skip the object itself

                if (isObstacle(go))
                {
                    BlockType goType = (BlockType)Enum.Parse(typeof(BlockType), go.name);   //BlockType of the game object
                    if (type == goType) //Only erase a block of the same type
                    {
                        if (level.RemoveBlockAtTarget(type, go.transform.position, go.transform.localScale.x))
                        {
                            Destroy(go);
                        }
                    }
                }
            }
            Debug.Log("these are the " + colliders.Length + " colliders: " + names);
        }
    }

    public bool isObstacle(GameObject gObject)
    {
        foreach (string name in blockNames)
        {
            if (gObject.name == name) return true;
        }
        return false;
    }

    public void save()
    {
        level.save(Path.Combine(Application.dataPath, "Levels/Resources/level-" + Application.loadedLevel + ".xml"));
        Debug.Log("saved to level-" + Application.loadedLevel + ".xml");
    }

    public void load()
    {
        TextAsset levelText = (TextAsset)Resources.Load("level-" + Application.loadedLevel, typeof(TextAsset));
        if (levelText)
        {
            level = Level.loadFromText(levelText.ToString());
        }
        loadBlockType(wall, level.Walls);
        loadBlockType(goal, level.Goals);
        loadBlockType(force, level.Forces);
        loadBlockType(planet, level.Planets);
    }

    public void increaseBlockSize()
    {
        cursor.transform.localScale = new Vector3(cursor.transform.localScale.x * 2, cursor.transform.localScale.y * 2, cursor.transform.localScale.z);
        altCursor.transform.localScale = cursor.transform.localScale;
    }

    public void decreaseBlockSize()
    {
        cursor.transform.localScale = new Vector3(cursor.transform.localScale.x / 2, cursor.transform.localScale.y / 2, cursor.transform.localScale.z);
        altCursor.transform.localScale = cursor.transform.localScale;
    }

    

    void loadBlockType(GameObject block, List<Block> blocks)
    {
        bool visible = block.renderer.enabled;
        block.renderer.enabled = true;
        Transform parent = block.transform.parent;
        block.transform.parent = null;
        foreach (Block b in blocks)
        {
            GameObject newBlock = (GameObject)Instantiate(block, b.position, block.transform.rotation);
            if (b.scale != 0) newBlock.transform.localScale = new Vector3(b.scale, b.scale, block.transform.localScale.z);
            //newBlock.transform.RotateAround(
            newBlock.name = block.name;

            // enable planet gravity here, because we don't want the palette one affecting the ship
            if (newBlock.name == "Planet")
            {
                Planet planet = (Planet)newBlock.GetComponent("Planet");
                planet.enabled = true;
                if (b.strength != 0) planet.gravity = b.strength;
            }

            if (newBlock.name == "Force")
            {
                ForceArea forceArea = (ForceArea)newBlock.GetComponent("ForceArea");
                if (b.strength != 0) forceArea.magnitude = b.strength;
            }
        }

        // Put your blocks back how you found them
        block.transform.parent = parent;
        block.renderer.enabled = visible;
    }
}
