using System;
using System.Collections.Generic;
using UnityEngine;

namespace XRC.Assignments.Meshes
{
    public class MyMeshUtil
    {
        /// <summary>
        /// Returns a list of vertices from the input mesh based on the region of interest.
        /// The method uses the mesh's bounds to search within the region of interest and add the indices for the vertices found to a list of selected indices 
        /// See Unity's Bounds class.
        /// </summary>
        /// <param name="inputMesh">The input mesh</param>
        /// <param name="regionOfInterest">The region within the bounds where vertices will be searched</param>
        /// <returns>List of indices indicating the selected vertices</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static List<int> SelectVertices(Mesh inputMesh, ModificationRegion regionOfInterest)
        {
            List<int> selectedIndices = new List<int>();

            // TODO - Implement according to summary
            // <solution>
            // Your code here
            // Loop through all vertices in the mesh
            switch (regionOfInterest)
            {
                case ModificationRegion.All:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        selectedIndices.Add(i);
                    }
                    break;
                case ModificationRegion.RightHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].x > inputMesh.bounds.center.x)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
                case ModificationRegion.TopHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].y > inputMesh.bounds.center.y)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
                case ModificationRegion.FrontHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].z > inputMesh.bounds.center.z)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
                case ModificationRegion.LeftHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].x < inputMesh.bounds.center.x)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
                case ModificationRegion.BottomHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].y < inputMesh.bounds.center.y)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
                case ModificationRegion.BackHalf:
                    for (int i = 0; i < inputMesh.vertexCount; i++)
                    {
                        if (inputMesh.vertices[i].z < inputMesh.bounds.center.z)
                        {
                            selectedIndices.Add(i);
                        }
                    }
                    break;
            }
            // </solution>

            return selectedIndices;
        }

        /// <summary>
         /// Transforms the selected vertices (indicated by the list of indices) of a mesh according to the input parameters; translation, rotation, scaling.
         /// This method uses Matrix4x4 to perform the transformations.
         /// </summary>
         /// <param name="vertices">All vertices of the mesh</param>
         /// <param name="selectedIndices">The set of indices indicating which subset of vertices should be transformed</param>
         /// <param name="translation">Translation in (x, y, z)</param>
         /// <param name="rotation"> Rotation in Euler angles</param>
         /// <param name="scale"> Scale</param>
         /// <returns>Array of vertices where the selected vertices have been transformed</returns>
         public static Vector3[] Transform(Vector3[] vertices, List<int> selectedIndices,
             Vector3 translation, Vector3 rotation, Vector3 scale)
         {
             Vector3[] transformedVertices = vertices;
 
 
             // TODO - Implement according to summary
             // <solution>
             // Your code here=
             Matrix4x4 matrix = Matrix4x4.TRS(translation, Quaternion.Euler(rotation), scale);

             foreach (int index in selectedIndices)
             {
                 transformedVertices[index] = matrix.MultiplyPoint3x4(vertices[index]);
             }
             // </solution>
             
             return transformedVertices;
         }

        /// <summary>
        /// Translate selected vertices
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="selectedIndices"></param>
        /// <param name="translation"></param>
        /// <returns>Array of vertices where the selected vertices have been transformed</returns>
        public static Vector3[] Translate(Vector3[] vertices, List<int> selectedIndices, Vector3 translation)
        {
            return Transform(vertices, selectedIndices, translation, Vector3.zero, Vector3.one);
        }

        /// <summary>
        /// Rotate selected vertices
        /// </summary> 
        /// <param name="vertices"></param>
        /// <param name="selectedIndices"></param>
        /// <param name="rotation"></param>
        /// <returns>Array of vertices where the selected vertices have been transformed</returns>
        public static Vector3[] Rotate(Vector3[] vertices, List<int> selectedIndices, Vector3 rotation)
        {
            return Transform(vertices, selectedIndices, Vector3.zero, rotation, Vector3.one);
        }
        
        // Scale selected vertices
        public static Vector3[] Scale(Vector3[] vertices, List<int> selectedIndices, Vector3 scale)
        {
            return Transform(vertices, selectedIndices, Vector3.zero, Vector3.zero, scale);
        }

        /// <summary>
        /// This method translates the selected vertices in the direction of its normals.
        /// </summary>
        /// <param name="vertices">Array of vertices</param>
        /// <param name="selectedIndices">List of indices indicating the selected vertices</param>
        /// <param name="normals">Array of mesh normals</param>
        /// <param name="magnitude">Scala value, the magnitude of the translation in normal direction</param>
        /// <returns>Array of vertices where the selected vertices have been transformed</returns>
        public static Vector3[] TranslateInNormalDirection(Vector3[] vertices, List<int> selectedIndices,
            Vector3[] normals, float magnitude)
        {
            Vector3[] transformedVertices = vertices;
            
            // TODO - Implement according to summary
            // <solution>
            // Your code here
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                int index = selectedIndices[i];
                transformedVertices[index] = vertices[index] + normals[index] * magnitude;
            }
            // </solution>
            
            return transformedVertices;
        }
    }
}