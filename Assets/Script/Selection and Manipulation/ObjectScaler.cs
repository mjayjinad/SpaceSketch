using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using xrc_assignments_project_g05.Scripts.Selection_and_Manipulation;

public class ObjectScaler : MonoBehaviour
{
    [Header("Gizmo Prefabs")]
    public GameObject scaleXGizmoPrefab;
    public GameObject scaleYGizmoPrefab;
    public GameObject scaleZGizmoPrefab;

    public bool isGrabbed = false;
    public float scalingSensitivity = 0.1f;
    public float smoothingSpeed = 5f; // Adjust this speed as needed


    [Header("Gizmo Properties")]
    private Vector3 initialGizmoPosition;
    private GameObject gizmoHolder; // Parent for gizmos
    private Transform interactedGizmo = null;
    private XRBaseInteractable scaleXGizmo;
    private XRBaseInteractable scaleYGizmo;
    private XRBaseInteractable scaleZGizmo;

    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Vector3 scaleBeforeScaling;
    private Quaternion initialRotation;
    private GizmoAxis currentScalingAxis = GizmoAxis.None;
    private XRGrabInteractable grabInteractable;
    private bool isScaling = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        FindObjectOfType<InputManager>()?.RegisterScaleAxisObject(this);
    }

    void Start()
    {
        initialScale = transform.localScale;
        initialRotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        if (isScaling && interactedGizmo != null)
        {
            ScaleObject();
        }
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnDestroy()
    {
        if (gizmoHolder) Destroy(gizmoHolder);
        FindObjectOfType<InputManager>()?.UnregisterScaleAxisObject(this);
    }

    public enum GizmoAxis
    {
        None,
        X,
        Y,
        Z
    }

    public void AttemptToActivateScale(InputAction.CallbackContext context)
    {
        bool isActive = context.ReadValueAsButton();
        if (isActive)
        {
            if (isGrabbed)
            {
                // Get the interactor that is currently grabbing this object
                XRBaseInteractor interactor = grabInteractable.selectingInteractor;

                if (interactor != null)
                {
                    IXRSelectInteractor selectableInteractor = grabInteractable.selectingInteractor;
                    // Force the release of the object
                    interactor.interactionManager.SelectExit(selectableInteractor, grabInteractable);
                }

                // Now the object is no longer grabbed, we can reset it
                ResetObject();

                ActivateGizmos(true);
                // Assign this object as the scaler for each gizmo
                scaleXGizmo.GetComponent<GizmoController>().AssignObjectScaler(this);
                scaleYGizmo.GetComponent<GizmoController>().AssignObjectScaler(this);
                scaleZGizmo.GetComponent<GizmoController>().AssignObjectScaler(this);
            }
            else
            {
                ActivateGizmos(false);
            }
        }
        else
        {
            ActivateGizmos(false);
        }
    }

    private void ResetObject()
    {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }

    private void ScaleObject()
    {
        // Unparent gizmo holder to avoid inheriting scale
        gizmoHolder.transform.parent = null;

        // Calculate the movement vector and corresponding scale change
        Vector3 movement = interactedGizmo.position - initialGizmoPosition;
        Vector3 scaleChange = movement * scalingSensitivity;

        // Set the initial target scale based on the current object's scale
        Vector3 targetScale = transform.localScale;

        // Apply the scale change only to the selected axis
        switch (currentScalingAxis)
        {
            case GizmoAxis.X:
                targetScale.x = Mathf.Max(0.05f, initialScale.x + scaleChange.x);
                break;
            case GizmoAxis.Y:
                targetScale.y = Mathf.Max(0.05f, initialScale.y + scaleChange.y);
                break;
            case GizmoAxis.Z:
                targetScale.z = Mathf.Max(0.05f, initialScale.z + scaleChange.z);
                break;
        }

        // Smoothly interpolate to the new scale
        Vector3 velocity = Vector3.zero;  // Define outside if you want to retain velocity between frames
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, smoothingSpeed * Time.deltaTime);

        // Reparent gizmo holder after scaling
        gizmoHolder.transform.parent = transform;

        // Reposition gizmos relative to the new scale of the object
        PositionGizmos();
    }

    private void ActivateGizmos(bool activate)
    {
        if (activate)
        {
            if (!gizmoHolder)
            {
                gizmoHolder = new GameObject("GizmoHolder");
                gizmoHolder.transform.position = transform.position;
                gizmoHolder.transform.rotation = transform.rotation;
                gizmoHolder.transform.parent = transform;
            }

            scaleXGizmo = Instantiate(scaleXGizmoPrefab, gizmoHolder.transform).GetComponent<XRBaseInteractable>();
            scaleYGizmo = Instantiate(scaleYGizmoPrefab, gizmoHolder.transform).GetComponent<XRBaseInteractable>();
            scaleZGizmo = Instantiate(scaleZGizmoPrefab, gizmoHolder.transform).GetComponent<XRBaseInteractable>();

            RegisterGizmo(scaleXGizmo);
            RegisterGizmo(scaleYGizmo);
            RegisterGizmo(scaleZGizmo);

            PositionGizmos();
        }
        else
        {
            if (gizmoHolder)
            {
                if (scaleXGizmo) Destroy(scaleXGizmo.gameObject);
                if (scaleYGizmo) Destroy(scaleYGizmo.gameObject);
                if (scaleZGizmo) Destroy(scaleZGizmo.gameObject);

                if (gizmoHolder) Destroy(gizmoHolder);
            }
        }
    }

    private void PositionGizmos()
    {
        // Calculate and set positions directly
        Bounds bounds = GetComponent<Renderer>().bounds;

        scaleXGizmo.transform.position = bounds.center + new Vector3(bounds.extents.x, 0, 0);
        scaleYGizmo.transform.position = bounds.center + new Vector3(0, bounds.extents.y, 0);
        scaleZGizmo.transform.position = bounds.center + new Vector3(0, 0, bounds.extents.z);
    }

    private void RegisterGizmo(XRBaseInteractable gizmo)
    {
        gizmo.selectEntered.AddListener(StartScaling);
        gizmo.selectExited.AddListener(StopScaling);
    }

    private void OnGrabbed(SelectEnterEventArgs arg)
    {
        initialPosition = transform.localPosition;
        isGrabbed = true;

        // Deactivate gizmos after clearing their references
        if (scaleXGizmo) scaleXGizmo.GetComponent<GizmoController>().ClearObjectScaler();
        if (scaleYGizmo) scaleYGizmo.GetComponent<GizmoController>().ClearObjectScaler();
        if (scaleZGizmo) scaleZGizmo.GetComponent<GizmoController>().ClearObjectScaler();

        ActivateGizmos(false);
    }

    private void OnReleased(SelectExitEventArgs arg)
    {
        isGrabbed = false;
    }

    private void StartScaling(SelectEnterEventArgs arg)
    {
        // Reposition gizmos relative to the new scale of the object
        PositionGizmos();

        // Set the object's collider to isTrigger
        Collider objectCollider = GetComponent<Collider>();
        if (objectCollider != null)
        {
            gameObject.GetComponent<MeshCollider>().convex = true;
            objectCollider.isTrigger = true;
        }

        var gizmoController = arg.interactableObject.transform.GetComponent<GizmoController>();
        if (gizmoController != null && gizmoController.currentScaler == this)
        {
            // Set the flag indicating that scaling is in progress
            isScaling = true;

            // Store a reference to the interacted gizmo
            interactedGizmo = arg.interactableObject.transform;

            // Capture the initial world position of the gizmo at the start of the scaling interaction
            initialGizmoPosition = interactedGizmo.position;

            // Determine which gizmo axis is being used
            if (arg.interactableObject == scaleXGizmo)
                currentScalingAxis = GizmoAxis.X;
            else if (arg.interactableObject == scaleYGizmo)
                currentScalingAxis = GizmoAxis.Y;
            else if (arg.interactableObject == scaleZGizmo)
                currentScalingAxis = GizmoAxis.Z;

            // Record the initial scale
            scaleBeforeScaling = transform.localScale;
        }
    }

    private void StopScaling(SelectExitEventArgs arg)
    {
        // Reposition gizmos relative to the new scale of the object
        PositionGizmos();

        // Revert the object's collider back to its original state
        Collider objectCollider = GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.isTrigger = false;
        }

        var gizmoController = arg.interactableObject.transform.GetComponent<GizmoController>();
        if (gizmoController != null && gizmoController.currentScaler == this)
        {
            isScaling = false;
            interactedGizmo = null;
            currentScalingAxis = GizmoAxis.None;

            // Use the GameObject's unique name (including the GUID)
            string uniqueObjectName = gameObject.name;

            ICommand scaleCommand = new ScaleObjectCommand(uniqueObjectName, scaleBeforeScaling, transform.localScale);
            CommandManager.Instance.ExecuteCommand(scaleCommand);
        }
    }
}
