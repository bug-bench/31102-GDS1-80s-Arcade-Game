using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InfiniteModeUIManager : MonoBehaviour
{
    [SerializeField] private InfiniteModeManager infiniteModeManager;
    [SerializeField] private Text helicopterText;
    [SerializeField] private Text rescuedText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text winText;
    [SerializeField] private InputActionReference restartGameAction;
    private bool isGameOver = false;
    private bool isGameWon = false;

    void Update()
    {
        if (infiniteModeManager == null) return;
        if (helicopterText != null)
        {
            helicopterText.text = $"In Helicopter: {infiniteModeManager.GetPickedUpSoldierCount()} / {infiniteModeManager.GetMaxPickedUpSoldiers()}";
        }
        if (rescuedText != null)
        {
            rescuedText.text = $"Rescued: {infiniteModeManager.GetRescuedSoldierCount()} / {infiniteModeManager.GetMaxRescuedSoldiers()}";
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverText != null) gameOverText.text = "Game Over! Press R to Restart.";
        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
    }

    public void ShowWin()
    {
        isGameWon = true;
        if (winPanel != null) winPanel.SetActive(true);
        if (winText != null) winText.text = "You Win!";
        if (winText != null) winText.gameObject.SetActive(true);
    }

    public void HideAllPanels()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        isGameOver = false;
        isGameWon = false;
    }

    private void OnEnable()
    {
        if (restartGameAction != null && restartGameAction.action != null)
            restartGameAction.action.Enable();
        if (restartGameAction != null && restartGameAction.action != null)
            restartGameAction.action.performed += OnRestartGame;
    }

    private void OnDisable()
    {
        if (restartGameAction != null && restartGameAction.action != null)
            restartGameAction.action.performed -= OnRestartGame;
        if (restartGameAction != null && restartGameAction.action != null)
            restartGameAction.action.Disable();
    }

    private void OnRestartGame(InputAction.CallbackContext context)
    {
        if (isGameOver && context.performed)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
