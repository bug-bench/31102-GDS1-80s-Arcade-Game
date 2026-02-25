using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button quitButton;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference menuControllerAction;

    private int selectedIndex = 0;
    private Button[] buttons;
    private bool canPress = true;
    private float inputDelay = 0.2f;
    private float lastInputTime = 0f;

    private void Awake()
    {
        buttons = new Button[] { level1Button, level2Button, quitButton };
    }

    private void OnEnable()
    {
        if (menuControllerAction != null && menuControllerAction.action != null)
            menuControllerAction.action.Enable();
    }

    private void OnDisable()
    {
        if (menuControllerAction != null && menuControllerAction.action != null)
            menuControllerAction.action.Disable();
    }

    private void Start()
    {
        UpdateButtonSelection();
        // Add listeners for mouse clicks
        level1Button.onClick.AddListener(OnLevel1Clicked);
        level2Button.onClick.AddListener(OnLevel2Clicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void Update()
    {
        if (menuControllerAction == null || menuControllerAction.action == null)
            return;

        Vector2 nav = menuControllerAction.action.ReadValue<Vector2>();
        bool selectPressed = menuControllerAction.action.triggered;

        // Navigation (Up/Down)
        if (Time.time - lastInputTime > inputDelay)
        {
            if (nav.y > 0.5f)
            {
                selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
                UpdateButtonSelection();
                lastInputTime = Time.time;
            }
            else if (nav.y < -0.5f)
            {
                selectedIndex = (selectedIndex + 1) % buttons.Length;
                UpdateButtonSelection();
                lastInputTime = Time.time;
            }
        }

        // Selection (Space/Enter)
        if (menuControllerAction.action.WasPressedThisFrame())
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            buttons[i].colors = colors;
        }
        buttons[selectedIndex].Select();
    }

    public void OnLevel1Clicked()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void OnLevel2Clicked()
    {
        SceneManager.LoadScene("InfiniteLevel");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
