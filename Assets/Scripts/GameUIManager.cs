using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text helicopterText;
    [SerializeField] private Text rescuedText;

    void Update()
    {
        if (gameManager == null) return;
        if (helicopterText != null)
        {
            helicopterText.text = $"In Helicopter: {gameManager.GetPickedUpSoldierCount()} / {gameManager.GetMaxPickedUpSoldiers()}";
        }
        if (rescuedText != null)
        {
            rescuedText.text = $"Rescued: {gameManager.GetRescuedSoldierCount()} / {gameManager.GetMaxRescuedSoldiers()}";
        }
    }
}
