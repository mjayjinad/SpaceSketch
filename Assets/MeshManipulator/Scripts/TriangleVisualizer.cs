using System.Collections.Generic;
using UnityEngine;

namespace XRC.Assignments.Meshes
{
    /// <summary>
    /// This script visualizes the mesh of its game object by drawing lines between each triangle vertex, using a line renderer
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class TriangleVisualizer : MonoBehaviour
    {
        private List<LineRenderer> m_LineRenderers;
        private Mesh m_Mesh;
        
        void Start()
        {
            m_Mesh = gameObject.GetComponent<MeshFilter>().mesh;
            m_LineRenderers = new List<LineRenderer>();
        }

        void LateUpdate()
        {
            // By doing the following in Update(), changes to the mesh during runtime are represented by the visualizer
            VisualizeTriangles(m_Mesh.vertices, m_Mesh.triangles);
        }

        /// <summary>
        /// This method is called by Update() every frame.
        /// The method calls SetupLineRenderer() and SetLineRendererPositions()
        /// </summary>
        /// <param name="vertices">Vertices</param>
        /// <param name="triangles">Triangle indices</param>
        private void VisualizeTriangles(Vector3[] vertices, int[] triangles)
        {
            // TODO - Implement method according to summary and instructions
            // <solution>]
            // Your code here
            int count = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 vertex1 = transform.TransformPoint(vertices[triangles[i]]);
                Vector3 vertex2 = transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 vertex3 = transform.TransformPoint(vertices[triangles[i + 2]]);

                if (m_LineRenderers.Count <= count)
                        SetupLineRenderer();
                SetLineRendererPositions(vertex1, vertex2, vertex3, i / 3);
                count++;
            }
            // </solution>
        }

        /// <summary>
        /// Instantiates a new game object and adds a LineRenderer component to it.
        /// The new game object is set as a child of the current game object.
        /// Each line renderer is named "TriangleLineRenderer".
        /// The LineRenderer has width 0.02 and the appropriate number of positions to draw a closed triangle
        /// The line renderer component is added to the m_LineRenderer list.
        /// </summary>
        private void SetupLineRenderer()
        {
            // TODO - Implement method according to summary and instructions
            // <solution>
            // Your code here

            GameObject lineRendererGameObject = new GameObject("TriangleLineRenderer");
            lineRendererGameObject.transform.parent = this.transform;
            LineRenderer lineRenderer = lineRendererGameObject.AddComponent<LineRenderer>();
           // lineRenderer.widthMultiplieThr = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            Color c1 = Color.red;
            Color c2 = new Color(1, 1, 1, 0);
            lineRenderer.SetColors(c1, c2);
            lineRenderer.positionCount =
                4; // Three points for the vertices of the triangle, and one more to close the triangle
            m_LineRenderers.Add(lineRenderer);
            // </solution>
        }


        /// <summary>
        /// This method sets the positions for the line renderer used to visualize the triangle
        /// </summary>
        /// <param name="vertex1">First vertex</param>
        /// <param name="vertex2">Second vertex</param>
        /// <param name="vertex3">Third vertex</param>
        /// <param name="lineRendererIndex">The index for the line renderer in the list of line renderers</param>
        private void SetLineRendererPositions(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, int lineRendererIndex)
        {
            // TODO - Implement method according to summary and instructions
            // <solution>
            // Your code here
            
            LineRenderer lineRenderer = m_LineRenderers[lineRendererIndex];
            lineRenderer.SetPosition(0, vertex1);
            lineRenderer.SetPosition(1, vertex2);
            lineRenderer.SetPosition(2, vertex3);
            lineRenderer.SetPosition(3, vertex1); // To close the triangle

            // </solution>
        }

    }
}