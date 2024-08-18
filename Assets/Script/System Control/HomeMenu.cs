using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using SpaceSketch.Scripts.Selection_and_Manipulation;

public class HomeMenu : MonoBehaviour
{
    [Header("[Input Actions]")]
    public InputActionReference menuAction;

    [Header("[Logic Reference]")]
    public SphereSelectInput sphereSelectInput;
    public ClockUIInteractionManager clockUIInteractionManager;

    [Header("[UI Elements]")]
    public Button shapesBtn;
    public Button settingsBtn;
    public Button quitBtn;

    [Header("[Object Parameters]")]
    public GameObject shapes;
    public GameObject settings;
    public GameObject xRRay;
    public GameObject createObject;
    public GameObject sphereInteractor;
    public GameObject homeMenuCanvas;
    public GameObject shapesMenu;
    public GameObject settingPage;

    private bool isActive = false;

    private void Start()
    {
        shapesBtn.onClick.AddListener(() => CreateShapes());
        settingsBtn.onClick.AddListener(() => SettingsPage());
        quitBtn.onClick.AddListener(() => QuitApplication());
    }

    private void OnEnable()
    {
        menuAction.action.performed += ActivateInteractor;

        menuAction.action.Enable();
    }

    private void OnDisable()
    {
        menuAction.action.performed -= ActivateInteractor;

        menuAction.action.Disable();
    }

    public void CreateShapes()
    {
        shapes.SetActive(true);
        sphereSelectInput.enabled = false;
        clockUIInteractionManager.enabled = false;
        sphereInteractor.SetActive(false);
        xRRay.SetActive(false);
        homeMenuCanvas.SetActive(false);
    }

    public void SettingsPage()
    {
        settings.SetActive(true);
        xRRay.SetActive(true);
        homeMenuCanvas.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void ActivateInteractor(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        if (isActive)
        {
            homeMenuCanvas.SetActive(true);
            xRRay.SetActive(true);
        }
        else
        {
            homeMenuCanvas.SetActive(false);
            shapesMenu.SetActive(false);
            settingPage.SetActive(false);
            xRRay.SetActive(false);
            clockUIInteractionManager.enabled = true;
        }
    }
}
