using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class MainMenu : MonoBehaviour
{
    public Button prototype1Button;
    public Button prototype2Button;
    public Button prototype3Button;
    public GameObject menuMain;

    private bool menuActive = false;

    private readonly MLPermissions.Callbacks permissionCallbacks = new MLPermissions.Callbacks();
    public MLVoiceIntentsConfiguration MenuVoiceIntentsConfiguration;

    private MagicLeapInputs magicLeapInputs;
    private MagicLeapInputs.ControllerActions controllerActions;

    private void Awake()
    {
        permissionCallbacks.OnPermissionGranted += OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied += OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain += OnPermissionDenied;
    }

    private void OnDestroy()
    {
        permissionCallbacks.OnPermissionGranted -= OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied -= OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain -= OnPermissionDenied;
    }



    void Start()
    {
        prototype1Button.onClick.AddListener(LoadPrototype1);
        prototype2Button.onClick.AddListener(LoadPrototype2);
        prototype3Button.onClick.AddListener(LoadPrototype3);
        menuMain.SetActive(true);

        magicLeapInputs = new MagicLeapInputs();
        magicLeapInputs.Enable();
        controllerActions = new MagicLeapInputs.ControllerActions(magicLeapInputs);
        controllerActions.Menu.performed += Menu_performed;


    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        menuActive = !menuActive;
        menuMain.SetActive(menuActive);
    }

    void LoadPrototype1()
    {
        SceneManager.LoadScene("Prototype1");
    }

    void LoadPrototype2()
    {
        SceneManager.LoadScene("Prototype2");
    }

    void LoadPrototype3()
    {
        SceneManager.LoadScene("Prototype3");
    }

    private void InitializeVoiceInput()
    {
        bool isVoiceEnabled = MLVoice.VoiceEnabled;
        if (isVoiceEnabled)
        {
            var result = MLVoice.SetupVoiceIntents(MenuVoiceIntentsConfiguration);
            if (result.IsOk)
            {
                MLVoice.OnVoiceEvent += MLVoiceOnOnVoiceEvent;
            }
            else
            {
                Debug.LogError("Voice could not initialize:" + result);
            }
        }
        else
        {
            UnityEngine.XR.MagicLeap.SettingsIntentsLauncher.LaunchSystemVoiceInputSettings();
            Application.Quit();
        }
    }


    private void MLVoiceOnOnVoiceEvent(in bool wassuccessful, in MLVoice.IntentEvent voiceevent)
    {
        if (wassuccessful)
        {
            if (voiceevent.EventID == 201)
            {
                Debug.Log("Menu opened");
                menuMain.SetActive(true);
            }
            if (voiceevent.EventID == 202)
            {
                Debug.Log("Menu closed");
                menuMain.SetActive(false);
            }
            
        }
    }




    private void OnPermissionGranted(string permission)
    {

        if (permission == MLPermission.VoiceInput)
            InitializeVoiceInput();
    }

    private void OnPermissionDenied(string permission)
    {
        Debug.LogError($"Failed to create Planes Subsystem due to missing or denied {MLPermission.SpatialMapping} permission. Please add to manifest. Disabling script.");
        enabled = false;
    }
}
