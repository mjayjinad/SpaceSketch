using UnityEngine;

namespace XRC.Assignments.Meshes
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshModifier : BaseMeshModifier
    {
        [Range(0.1f, 20f)]
        [SerializeField]
        private float m_Speed = 0.1f;

        [Range(-2.0f, 2.0f)]
        [SerializeField]
        private float m_NormalDirectionMultiplier = 1f;


        [SerializeField]
        private bool m_DoOscillation;

        public enum ModificationType
        {
            Transform,
            Translate,
            Rotate,
            Scale,
            TranslateInNormalDirection
        }

        [SerializeField]
        private ModificationType m_ModificationType;

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"m_ModificationRegion: {m_ModificationRegion}");

            m_SelectedIndices = MyMeshUtil.SelectVertices(m_OriginalMesh, m_ModificationRegion);

            float oscillatorValue = 1.0f;
            if (m_DoOscillation)
            {
                oscillatorValue = Mathf.Cos(m_Speed * Time.time);
            }
            switch (m_ModificationType)
            {
                case ModificationType.Transform:
                    m_Mesh.vertices = MyMeshUtil.Transform(m_OriginalMesh.vertices, m_SelectedIndices, m_Translation * oscillatorValue, m_Rotation * oscillatorValue, m_Scale * oscillatorValue);
                    break;
                case ModificationType.Translate:
                    m_Mesh.vertices = MyMeshUtil.Translate(m_OriginalMesh.vertices, m_SelectedIndices, m_Translation * oscillatorValue);
                    break;
                case ModificationType.Rotate:
                    m_Mesh.vertices = MyMeshUtil.Rotate(m_OriginalMesh.vertices, m_SelectedIndices, m_Rotation * oscillatorValue);
                    break;
                case ModificationType.Scale:
                    m_Mesh.vertices = MyMeshUtil.Scale(m_OriginalMesh.vertices, m_SelectedIndices, m_Scale * oscillatorValue);
                    break;
                case ModificationType.TranslateInNormalDirection:
                    m_Mesh.vertices = MyMeshUtil.TranslateInNormalDirection(m_OriginalMesh.vertices, m_SelectedIndices, m_Normals, m_NormalDirectionMultiplier * oscillatorValue);
                    break;
            }

            // Recalculate properties for live mesh*/
            m_Mesh.RecalculateNormals();
            m_Mesh.RecalculateBounds();
            m_Mesh.RecalculateTangents();
        }
    }
}