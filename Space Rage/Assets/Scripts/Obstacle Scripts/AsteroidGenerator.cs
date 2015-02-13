using UnityEngine;
using System.Collections;

public class AsteroidGenerator : MonoBehaviour {

    float scale = 30;
    int iterationsLimit = 10;
    int iterations;
    ArrayList cubes = new ArrayList();
    Material m;

	// Use this for initialization
	void Start () {
        m = (Material)Resources.Load("white", typeof(Material));
        iterations = Random.Range(2, iterationsLimit);
        buildAroundCoordinate(0, 0, 0, true);
        gameObject.rigidbody.velocity = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        gameObject.rigidbody.angularVelocity = new Vector3(0, 0, Random.Range(-4, 4));
	}

    private void buildAroundCoordinate(int centerX, int centerY, int count, bool centerIsEmpty)
    {
        if (centerIsEmpty) createCube(centerX, centerY);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x != 0 || y != 0) && Random.Range(0, 2) == 0)
                {
                    int cubeX = x + centerX;
                    int cubeY = y + centerY;

                    if (createCube(cubeX, cubeY) && count < iterations)
                        buildAroundCoordinate(cubeX, y + cubeY, count + 1, false);
                }
            }
        }
    }


    private bool createCube(float x, float y)
    {
        return createCube(new Vector2(x, y));
    }
    private bool createCube(int x, int y)
    {
        return createCube(new Vector2(x, y));
    }

    private bool createCube(Vector2 position)
    {
        if (cubes.Contains(position)) return false;
        cubes.Add(position);
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.parent = this.transform;
        g.transform.localPosition = new Vector3(position.x * scale, position.y * scale, 0);
        g.transform.localScale = new Vector3(scale, scale, scale);
        g.renderer.material = m;
        g.renderer.castShadows = false;
        g.renderer.receiveShadows = false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
	
	}
}
