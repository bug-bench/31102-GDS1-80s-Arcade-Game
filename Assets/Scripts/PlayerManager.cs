using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    public GameObject[] pickedUpSoldiers = new GameObject[3];
    private int pickedUpCount = 0;    

    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private AudioSource pickupAudioSource;
    [SerializeField] private AudioClip pickupClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (uiManager != null) uiManager.ShowGameOver();
            gameObject.SetActive(false); 
            Debug.Log("Game Over: Player hit an obstacle!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            if (pickedUpCount >= pickedUpSoldiers.Length)
            {
                Debug.Log("Cannot pick up more soldiers.");
                return;
            }

            if (SoldierManager.Instance != null)
            {
                SoldierManager.Instance.RemoveFromDeployed(collision.gameObject);
            }
            pickedUpSoldiers[pickedUpCount] = collision.gameObject;
            pickedUpCount++;
            collision.gameObject.SetActive(false);
            if (pickupAudioSource != null && pickupClip != null)
            {
                pickupAudioSource.PlayOneShot(pickupClip);
            }
            Debug.Log("Soldier collected. Total picked up: " + pickedUpCount);
        }

        if (collision.gameObject.CompareTag("Tent"))
        {
            MedicTentManager tentManager = collision.gameObject.GetComponent<MedicTentManager>();
            if (tentManager != null)
            {
                int deposited = 0;
                for (int i = 0; i < pickedUpSoldiers.Length; i++)
                {
                    GameObject soldier = pickedUpSoldiers[i];
                    if (soldier != null)
                    {
                        if (tentManager.TryDepositSoldier(soldier))
                        {
                            pickedUpSoldiers[i] = null;
                            deposited++;
                        }
                    }
                }
                pickedUpCount = 0;
                Debug.Log($"{deposited} soldier/s dropped off");
            }
            else
            {
                Debug.Log("no soldiers to drop off.");
            }
            if (uiManager != null)
            {
                var gameManager = GameObject.FindFirstObjectByType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.CheckWinCondition(uiManager);
                }
            }
        }
    }
}
