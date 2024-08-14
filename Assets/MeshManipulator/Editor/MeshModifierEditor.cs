using UnityEditor;
using XRC.Assignments.Meshes;

namespace XRC.Assignments.Meshes
{
    /// <summary>
    /// Custom editor script for MeshModifier component
    /// </summary>
    [CustomEditor(typeof(MeshModifier))]
    public class MeshModifierEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MeshModifier.ModificationType modificationType = (MeshModifier.ModificationType)serializedObject.FindProperty("m_ModificationType").enumValueFlag;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ModificationType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ModificationRegion"));

            switch (modificationType)
            {
                case MeshModifier.ModificationType.Transform:
                    ShowTranslationGUI();
                    ShowRotationGUI();
                    ShowScaleGUI();
                    SerializedProperty doOscillation = serializedObject.FindProperty("m_DoOscillation");
                    doOscillation.boolValue = false;
                    break;

                case MeshModifier.ModificationType.Translate:
                    ShowTranslationGUI();
                    ShowOscillationGUI();
                    break;

                case MeshModifier.ModificationType.Rotate:
                    ShowRotationGUI();
                    ShowOscillationGUI();
                    break;
                case MeshModifier.ModificationType.Scale:
                    ShowScaleGUI();
                    ShowOscillationGUI();
                    break;

                case MeshModifier.ModificationType.TranslateInNormalDirection:
                    ShowTranslationInNormalDirectionGUI();
                    ShowOscillationGUI();
                    break;

                default:
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }

        void ShowTranslationGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Translation"));
        }

        void ShowRotationGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Rotation"));
        }

        void ShowScaleGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Scale"));
        }

        void ShowTranslationInNormalDirectionGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NormalDirectionMultiplier"));
        }

        void ShowOscillationGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DoOscillation"));
            SerializedProperty doOscillation = serializedObject.FindProperty("m_DoOscillation");
            if (doOscillation.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Speed"));
            }
        }
    }
}