using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMeshCreator {

	public static Mesh CreateCustomQuadMesh(Vector3[] vertices)
    {
        if (vertices.Length != 4)   // not 4 vertices
            return null;

        Mesh mesh = new Mesh();
        mesh.name = "CustomMesh";
        mesh.Clear();

        mesh.vertices = new Vector3[4] {
            new Vector3(vertices[0].x, vertices[0].y, 0f),
            new Vector3(vertices[1].x, vertices[1].y, 0f),
            new Vector3(vertices[2].x, vertices[2].y, 0f),
            new Vector3(vertices[3].x, vertices[3].y, 0f)
        };

        mesh.uv = new Vector2[4];
        mesh.uv[0] = new Vector2(0, 0);
        mesh.uv[1] = new Vector2(1, 0);
        mesh.uv[2] = new Vector2(0, 1);
        mesh.uv[3] = new Vector2(1, 1);

        mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

        return mesh;
    }
}
