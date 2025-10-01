using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool _isPaused = false;
    public InputActionAsset playerInputs;
    private InputAction _pauseInput;

    int _stars = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        _pauseInput = InputSystem.actions["Pause"];
    }

    public void AddStar()
    {
        _stars++;
        Debug.Log("Estrellas recogidas: " + _stars);
    }

    void Update()
    {
        if (_pauseInput.WasPressedThisFrame())
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (_isPaused)
        {
            Time.timeScale = 1;
            GUIManager.Instance.ChangeCanvasStatus(GUIManager.Instance._pauseCanvas, false);
            playerInputs.FindActionMap("Player").Enable();
            _isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            GUIManager.Instance.ChangeCanvasStatus(GUIManager.Instance._pauseCanvas, true);
            playerInputs.FindActionMap("Player").Disable();
            _isPaused = true;
        }
    }
}
