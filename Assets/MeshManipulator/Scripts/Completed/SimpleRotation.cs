using UnityEngine;

namespace XRC.Assignments.Meshes
{
    public class SimpleRotation : MonoBehaviour
    {
        [SerializeField]
        private float m_AngleX, m_AngleY, m_AngleZ;

        // Start is called before the first frame update
        void Start()
        {
            // float speed = 10f;
            // m_AngleX = speed;
            // m_AngleY = speed;
            // m_AngleZ = speed;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(m_AngleX * Time.deltaTime, m_AngleY * Time.deltaTime, m_AngleZ * Time.deltaTime, Space.World);
        }
    }
}