using UnityEngine;

public class ObjectAttachment : MonoBehaviour
{
    public Rigidbody attachedRigidbody; // Rigidbody of the object to attach
    private FixedJoint joint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Controller")) // Use appropriate tag
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = collision.rigidbody;
            joint.breakForce = 1000f; // Set your pull force threshold here
        }
    }

    private void OnJointBreak(float breakForce)
    {
        // Joint broke due to high force
        Destroy(joint); // Clean up the joint component
        // Additional logic when objects are separated
    }
}
