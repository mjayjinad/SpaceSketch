using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using SpaceSketch.Scripts.System_Control;

namespace SpaceSketch.Scripts.Selection_and_Manipulation
{
    public class SphereSelect : MonoBehaviour
    {
        [SerializeField]
        private XRBaseInteractor interactor;
        [SerializeField]
        private Vector3 offset = Vector3.zero;

        private GameObject tempParent;
        private SelectionSphereFeedback selectionSphereFeedback;

        private void Awake()
        {
            if (interactor == null)
            {
                interactor = GetComponent<XRBaseInteractor>();
            }

            selectionSphereFeedback = GetComponentInChildren<SelectionSphereFeedback>();
        }

        private void Update()
        {
            // Keep the interactor's attach transform position updated
            interactor.attachTransform.localPosition = offset;
        }

        public void AdjustSelectionSphereRadius(float adjustment)
        {
            selectionSphereFeedback?.AdjustSphereRadius(adjustment);
        }

        public void SelectObjects()
        {
            List<GameObject> selectedObjects = SelectionManager.Instance.allSelectedObject;

            if (selectedObjects.Count == 1)
            {
                // Single Object Selection
                SelectionManager.Instance.RegisterSelectedObject(selectedObjects[0]);
                ReleaseGroup();
                SelectionManager.Instance.ClearSelectedObjects();
            }
            else if (selectedObjects.Count > 1)
            {
                // Multiple Object Selection
                GroupSelectedObjects(selectedObjects.ToArray());
            }
        }

        private void GroupSelectedObjects(GameObject[] gameobjects)
        {
            // Ensure tempParent is initialized and clear previous grouping if any.
            if (tempParent != null)
            {
                Destroy(tempParent);
            }

            tempParent = new GameObject("TempParent");

            // Calculate the bounds that encompass all objects
            Bounds groupBounds = new Bounds(Vector3.zero, Vector3.zero);
            bool hasBounds = false;
            foreach (var selectedObj in gameobjects)
            {
                Collider objCollider = selectedObj.GetComponent<Collider>();
                if (objCollider != null)
                {
                    if (hasBounds)
                    {
                        groupBounds.Encapsulate(objCollider.bounds);
                    }
                    else
                    {
                        groupBounds = new Bounds(objCollider.bounds.center, objCollider.bounds.size);
                        hasBounds = true;
                    }
                }
            }

            // Set tempParent position and rotation to average of selected objects.
            tempParent.transform.position = groupBounds.center;
            tempParent.transform.rotation = Quaternion.identity;

            // Create a new collider that encompasses all objects
            BoxCollider groupCollider = tempParent.AddComponent<BoxCollider>();
            groupCollider.center = Vector3.zero; // Centered on the tempParent
            groupCollider.size = groupBounds.size;

            XRGrabInteractable tempParentInteractable = tempParent.AddComponent<XRGrabInteractable>();
            tempParentInteractable.useDynamicAttach = true;
            var tempRigidBody = tempParent.GetComponent<Rigidbody>();
            tempRigidBody.useGravity = false;
            tempRigidBody.isKinematic = true;
            tempParent.AddComponent<Delete_Object>();
            tempParent.AddComponent<DuplicateObject>();
            tempParent.AddComponent<ColorSelectorFeedback>();

            tempParentInteractable.colliders.Clear();
            tempParentInteractable.colliders.Add(groupCollider);

            // Attach selected objects to tempParent and disable their XRGrabInteractable components.
            foreach (GameObject go in gameobjects)
            {
                GameObject obj = go.gameObject;
                SelectionManager.Instance.RegisterSelectedObject(obj);
                XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    interactable.enabled = false;
                    obj.transform.SetParent(tempParent.transform);
                }
            }

            // Refresh the XRGrabInteractable by disabling and re-enabling it.
            StartCoroutine(RefreshGrabInteractable(tempParentInteractable.gameObject));

            interactor.StartManualInteraction(tempParentInteractable);

            // Clear the selection as they are now grouped.
            SelectionManager.Instance.ClearSelectedObjects();
        }

        public void ReleaseGroup()
        {
            if (tempParent == null) return;

            foreach (Transform child in tempParent.transform)
            {
                StartCoroutine(RefreshGrabInteractable(child.gameObject));
            }

            if (interactor.hasSelection)
            {
                interactor.EndManualInteraction();
            }

            tempParent.transform.DetachChildren();
            Destroy(tempParent);
        }

        private IEnumerator RefreshGrabInteractable(GameObject obj)
        {
            XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
            if (interactable != null)
            {
                // Wait for the end of the frame to ensure all physics updates are completed
                yield return new WaitForEndOfFrame();
                interactable.enabled = false;
                yield return new WaitForEndOfFrame(); // Wait another frame after disabling
                interactable.enabled = true;
            }
        }
    }
}
