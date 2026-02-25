using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MedicTentManager medicTentManager;
    [SerializeField] private InputActionReference resetGameAction;
    [SerializeField] private InputActionReference quitToMenuAction;


    public int GetPickedUpSoldierCount()
    {
        if (playerManager == null) return 0;
        int count = 0;
        foreach (var soldier in playerManager.pickedUpSoldiers)
        {
            if (soldier != null) count++;
        }
        return count;
    }

    public int GetRescuedSoldierCount()
    {
        if (medicTentManager == null) return 0;
        return medicTentManager.GetDepositCount();
    }

    public int GetMaxPickedUpSoldiers()
    {
        if (playerManager == null || playerManager.pickedUpSoldiers == null) return 0;
        return playerManager.pickedUpSoldiers.Length;
    }

    public int GetMaxRescuedSoldiers()
    {
        if (medicTentManager == null || medicTentManager.droppedOffSoldiers == null) return 0;
        return medicTentManager.droppedOffSoldiers.Length;
    }

    private void OnEnable()
    {
        if (resetGameAction != null && resetGameAction.action != null)
            resetGameAction.action.Enable();
        if (quitToMenuAction != null && quitToMenuAction.action != null)
            quitToMenuAction.action.Enable();
    }

    private void OnDisable()
    {
        if (resetGameAction != null && resetGameAction.action != null)
            resetGameAction.action.Disable();
        if (quitToMenuAction != null && quitToMenuAction.action != null)
            quitToMenuAction.action.Disable();
    }

    private void FixedUpdate()
    {
        if (resetGameAction != null && resetGameAction.action != null && resetGameAction.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Game Reset!");
        }
        if (quitToMenuAction != null && quitToMenuAction.action != null && quitToMenuAction.action.triggered)
        {
            SceneManager.LoadScene("StartScreen");
            Debug.Log("Quit to StartScreen!");
        }
    }

    public void CheckWinCondition(GameUIManager uiManager)
    {
        if (GetRescuedSoldierCount() >= GetMaxRescuedSoldiers() && GetMaxRescuedSoldiers() > 0)
        {
            uiManager.ShowWin();
        }
    }
}
