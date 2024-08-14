using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Video;

namespace xrc_assignments_project_g05.Scripts.System_Control
{
    public class DocumentationPanelController : MonoBehaviour
    {
        public InputActionReference documentationAction;
        public GameObject documentationCanvas;
        private bool isActive = false;
        
        private void Start()
        {
        }

        private void OnEnable()
        {
            documentationAction.action.performed += ActivateInteractor;
            documentationAction.action.Enable();
            Debug.Log("LEFT GRIP ENABLE");
        }

        private void OnDisable()
        {
            documentationAction.action.performed -= ActivateInteractor;
            documentationAction.action.Disable();
        }

        private void ActivateInteractor(InputAction.CallbackContext context)
        {
            isActive = !isActive;
            if (isActive)
            {
                documentationCanvas.SetActive(true);
            }
            else
            {
                documentationCanvas.SetActive(false);
            }
        }
    }
}
