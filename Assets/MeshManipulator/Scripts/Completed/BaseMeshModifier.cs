using System.Collections.Generic;
using UnityEngine;

namespace XRC.Assignments.Meshes
{
    [RequireComponent(typeof(MeshFilter))]
    public class BaseMeshModifier : MonoBehaviour
    {
        protected Mesh m_Mesh;
        protected Vector3[] m_Vertices;
        protected Vector3[] m_Normals;
        protected int[] m_Triangles;
        protected List<int> m_SelectedIndices;
        protected Mesh m_OriginalMesh;

        [SerializeField]
        protected ModificationRegion m_ModificationRegion;

        [SerializeField]
        protected Vector3 m_Translation = Vector3.zero;
        
        [SerializeField]
        protected Vector3 m_Rotation = Vector3.zero;

        [SerializeField]
        protected Vector3 m_Scale = Vector3.one;

        protected virtual void Start()
        {
            m_Mesh = GetComponent<MeshFilter>().mesh;
            m_OriginalMesh = CopyMesh(m_Mesh);
            
            m_Vertices = m_Mesh.vertices;
            m_Triangles = m_Mesh.triangles;
            m_Normals = m_Mesh.normals;
            m_Triangles = m_Mesh.triangles;
        }
        
        /// <summary>
        /// Copies a mesh by constructing a new mesh with the same properties.
        /// Used to maintain a copy of the original mesh, before modifications
        /// </summary>
        /// <param name="sourceMesh"></param>
        /// <returns>The destination mesh, which is a copy of the source mesh</returns>
        private Mesh CopyMesh(Mesh sourceMesh)
        {
            Mesh destinationMesh = new Mesh();
            destinationMesh.vertices = sourceMesh.vertices;
            destinationMesh.triangles = sourceMesh.triangles;
            destinationMesh.normals = sourceMesh.normals;
            destinationMesh.bounds = sourceMesh.bounds;
            return destinationMesh;
        }
    }
}