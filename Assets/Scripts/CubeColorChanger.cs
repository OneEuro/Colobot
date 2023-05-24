using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColorChanger : MonoBehaviour
{

    // The side of the cube to paint
    public Vector3 sideToPaint;

    // The new color for the side
    public Color newColor;

    private MeshFilter cubeMeshFilter;
    public CharacterController characterController;
    private Mesh cubeMesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color[] colors;
    private Color[] newColors;

    

    void Start()
    {
        // Get the mesh filter component from the cube game object
        cubeMeshFilter = gameObject.GetComponent<MeshFilter>();
        characterController.OnVectorChanged += HandleVectorChanged;
        // Get the mesh from the cube's mesh filter
        cubeMesh = cubeMeshFilter.mesh;

        // Get the vertices and normals arrays from the cube's mesh
        vertices = cubeMesh.vertices;
        normals = cubeMesh.normals;
        colors = cubeMesh.colors;
        newColors = new Color[vertices.Length];
         
    }

    private void HandleVectorChanged(Vector3 newVector)
    {
        sideToPaint = newVector;
        Debug.Log("sideToPaint is assigned and it equal = " + newVector);
        ChangedSideColor();
    }

    public void ChangeBackColor() {

        for (int i = 0; i < vertices.Length; i++)
            {
                newColors[i] = Color.green;
            }
        
        cubeMesh.SetColors(newColors);
        cubeMeshFilter.mesh = cubeMesh;

    }

    void ChangedSideColor() {
        // Check if the cube has a mesh filter
        if (cubeMeshFilter != null && sideToPaint != Vector3.zero )
        {
            
            // Задаем цвета вершинам
            for (int i = 0; i < vertices.Length; i++)
            {
                newColors[i] = new Color(1f, 1f, 1f, 0.5f);  // Присваиваем каждой вершине прозрачный цвет
            }

            // Устанавливаем цвета вершин в меш
            cubeMesh.SetColors(newColors);
            

            // Iterate through the vertices array
            for (int i = 0; i < vertices.Length; i++)
            {

                Vector3 vertex = vertices[i];
              
                // Calculate the world space position of the current vertex
                Vector3 vertexWorldPos = gameObject.transform.TransformPoint(vertices[i]);

                // Calculate the world space normal direction of the current vertex
                Vector3 normalWorldDir = gameObject.transform.TransformDirection(normals[i]);

                // Check if the normal direction matches the side to paint
                if (Vector3.Dot(normalWorldDir.normalized, sideToPaint.normalized) > 0.9f)
                {
                    // Set the vertex color of the matching side to the new color
                    newColors[i] = newColor;
                    Debug.Log("new color is assigned");
                }
                 
            }
            cubeMesh.SetColors(newColors); 

            // Update the modified mesh with the new vertex colors
            cubeMeshFilter.mesh = cubeMesh;
        }
    }

}
