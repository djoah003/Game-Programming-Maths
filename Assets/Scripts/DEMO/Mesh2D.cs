using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject {

	// A 2D vertex
	[System.Serializable]
	public class Vertex {
		public Vector3 point;
		public Vector3 normal;
		public float u;
	}
	public int[] lineIndices;
	public Vertex[] vertices;

	public int VertexCount => vertices.Length;
	public int LineCount => lineIndices.Length;
}
