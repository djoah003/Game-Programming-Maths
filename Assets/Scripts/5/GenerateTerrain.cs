using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateTerrain : MonoBehaviour
{
  [System.Serializable]
  public class NoiseParams
  {
    public float FrequencyScale;
    public float AmplitudeScale;
  }
  
  
  [Range(1f,1000f)] [SerializeField]  private float size = 100f;
  [Range(2,255)] [SerializeField] private int segments = 100;
  [Range(0.1f, 100f)] [SerializeField] private float amplitudeScale;
  //[Range(0.1f, 100f)] [SerializeField] private float frequencyScale;
  [SerializeField] private NoiseParams[] NoiseLayers;
  
  [SerializeField] private bool perlin;
  [SerializeField] private bool ClampBelowValue;
  [SerializeField] private float ClampValue;
  [Range(0.1f, 100f)] [SerializeField] private float textureScale;
  private Mesh _mesh = null;


  void GenerateMesh()
  {
    if (_mesh == null)
      _mesh = new Mesh();
    else
      _mesh.Clear();
    
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();


    for (int y_seg = 0; y_seg <= segments; y_seg++)
    {
      for (int x_seg = 0; x_seg <= segments; x_seg++)
      {
        // Vertices
        float x = x_seg * (size / (float)segments);
        float z = y_seg * (size / (float)segments);

        float y = 0;
        for (int i = 0; i < NoiseLayers.Length; i++)
        {
          y += (Mathf.PerlinNoise(x / NoiseLayers[i].FrequencyScale, z / NoiseLayers[i].FrequencyScale -.5f)*
                NoiseLayers[i].AmplitudeScale);
        }

        if (ClampBelowValue && y < ClampValue)
          y = ClampValue;
        
        float uv_u = x_seg / (float)segments;
        float uv_v = y_seg / (float)segments;
        uv.Add(new Vector2(uv_u, uv_v));
        
        Vector3 v = new Vector3(x, y, z);
        vertices.Add(v);
        //Gizmos.DrawSphere(v, 0.1f);
      }
    }

    for (int y_seg = 0; y_seg < segments; y_seg++)
    {
      for (int x_seg = 0; x_seg < segments; x_seg++)
      {
        // Triangles
        int TopLeft = x_seg + y_seg * (segments + 1);
        int TopRight = TopLeft + 1;
        int BotLeft = TopLeft + segments + 1;
        int BotRight = BotLeft + 1;
        
        // 1st
        triangles.Add(TopLeft);
        triangles.Add(BotLeft);
        triangles.Add(TopRight);
        // 2nd
        triangles.Add(TopRight);
        triangles.Add(BotLeft);
        triangles.Add(BotRight);
      }
    }
    
    _mesh.SetVertices(vertices);
    _mesh.SetTriangles(triangles, 0);
    _mesh.SetUVs(0, uv);
    _mesh.RecalculateNormals();
    GetComponent<MeshFilter>().sharedMesh = _mesh;
    
    // After assigning the material to the mesh renderer:
    MeshRenderer renderer = GetComponent<MeshRenderer>();
    renderer.sharedMaterial.mainTextureScale = new Vector2(textureScale, textureScale);
    
    Debug.Log("Vertices count: " + vertices.Count);
    Debug.Log("UV count: " + uv.Count);

  }
  

  private void OnValidate()
  {
    GenerateMesh();
  }
}
