using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
    Mesh mesh;
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] uv;
    public int[] triangles;
    public Color32[] colors;
    public bool moving = false;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors32 = colors;
        mesh.uv = uv;
        mesh.normals = normals;
	}
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            float movement = Time.deltaTime * 5;
            if (vertices[1].y < 1)
            {
                //vertices[0].y += movement;
                //vertices[1].y += movement;
                //vertices[2].y += movement;
                //vertices[3].y += movement;
                //vertices[4].y += movement;

				for (int i = 0; i < vertices.Length; i++) {
					Vector3 vertex = vertices[i];
					Vector3 target = new Vector3(vertex.x, 1, vertex.z);

					vertex = Vector3.Lerp(vertex, target, movement);
					if (1 - vertex.y < .05f) {
						vertex.y = 1;
					}
					vertices[i] = vertex;
				}
            }
            //else if (vertices[0].y < 1)
            //{
            //    vertices[0].y += movement;
            //    vertices[2].y += movement;
            //}
            else
            {
                vertices[0].y = .5f;
                vertices[1].y = .5f;
                vertices[2].y = .5f;
                vertices[3].y = 0;
                vertices[4].y = 0;
            }
        }
        mesh.vertices = vertices;
	}
}
