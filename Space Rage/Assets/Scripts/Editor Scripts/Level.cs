﻿using UnityEngine;using System.Collections;using System.Collections.Generic;using System.Xml;using System.Xml.Serialization;using System.IO;[XmlRoot("Level")]public class Level {	[XmlArray("Walls"), XmlArrayItem("Wall")]	public List<Block> Walls = new List<Block>();	[XmlArray("Goals"), XmlArrayItem("Goal")]	public List<Block> Goals = new List<Block>();	[XmlArray("Forces"), XmlArrayItem("Force")]	public List<Block> Forces = new List<Block>();	[XmlArray("Planets"), XmlArrayItem("Planet")]	public List<Block> Planets = new List<Block>();	public Level() {	}	public void Start() {	}	public void Update(string name) {	}	public bool AddBlockAtTarget(BlockType type, Vector3 position, float scale = 1) {		switch (type) {		case BlockType.Wall:			return tryAddBlock(position, scale, Walls);		case BlockType.Goal:			return tryAddBlock(position, scale, Goals);		case BlockType.Force:			return tryAddBlock(position, scale, Forces);		case BlockType.Planet:			return tryAddBlock(position, scale, Planets);		default:			return false;		}	}	public bool RemoveBlockAtTarget(BlockType type, Vector3 position, float scale = 0) {		switch (type) {		case BlockType.Wall:			return tryRemoveBlock(position, scale, Walls);		case BlockType.Goal:			return tryRemoveBlock(position, scale, Goals);		case BlockType.Force:			return tryRemoveBlock(position, scale, Forces);		case BlockType.Planet:			return tryRemoveBlock(position, scale, Planets);		default:			return false;		}	}    public bool HasBlockAtTarget(BlockType type, Vector3 position, float scale = 0)    {        switch (type)        {            case BlockType.Wall:                return hasBlock(position, Walls);            case BlockType.Goal:                return hasBlock(position, Goals);            case BlockType.Force:                return hasBlock(position, Forces);            case BlockType.Planet:                return hasBlock(position, Planets);            default:                return false;        }    }	public Block getBlock(Vector3 position) {		Block result = getBlockByPosition(position, Walls);        if (result == null) result = getBlockByPosition(position, Goals);        if (result == null) result = getBlockByPosition(position, Forces);        if (result == null) result = getBlockByPosition(position, Planets);        return result;	}    private bool hasBlock(Vector3 position, List<Vector3> positions)    {        return positions.Contains(position);    }	private bool hasBlock(Vector3 position, List<Block> walls) {		foreach (Block w in walls) {			if (w.position == position) return true;		}		return false;	}    private bool tryAddBlock(Vector3 position, List<Vector3> positions)    {        if (!hasBlock(position, positions))        {            positions.Add(position);            return true;        }        return false;    }	private bool tryRemoveBlock(Vector3 position, List<Vector3> positions) {        if (hasBlock(position, positions))		{			positions.Remove(position);			return true;		}		return false;	}	private bool tryAddBlock(Vector3 position, float scale, List<Block> walls)	{		if (!hasBlock(position, walls))		{			Block w = new Block();			w.position = position;			if (scale != 0) w.scale = scale;			walls.Add(w);			return true;		}		return false;	}		private bool tryRemoveBlock(Vector3 position, float scale, List<Block> walls) {		if (hasBlock(position, walls))		{			walls.Remove(getBlockByPosition(position, walls));			return true;		}		return false;	}	private Block getBlockByPosition(Vector3 position, List<Block> walls) {		foreach (Block w in walls) {			if (w.position == position) return w;		}		return null;	}	public void save(string path) 	{		var serializer = new XmlSerializer(typeof(Level));		using(var stream = new FileStream(path, FileMode.Create))		{			serializer.Serialize(stream, this);		}	}	public static Level load(string path)	{		var serializer = new XmlSerializer(typeof(Level));		using(var stream = new FileStream(path, FileMode.Open))		{			return serializer.Deserialize(stream) as Level;		}	}	public static Level loadFromText(string text)	{		var serializer = new XmlSerializer(typeof(Level));		return serializer.Deserialize(new StringReader(text)) as Level;	}}public class Block {	public BlockType type;	public Vector3 position;	public float scale;	public float rotation = 0;	public float strength = 0;	public bool collision;	public string color = "default";}