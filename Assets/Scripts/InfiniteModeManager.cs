using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InfiniteModeManager : MonoBehaviour {
    [SerializeField] private InputActionReference resetGameAction;
    [SerializeField] private InputActionReference quitToMenuAction;
    private bool isGameWon = false;
    private bool isGameOver = false;
    private PlayerManager playerManager;
    private MedicTentManager medicTentManager;
    [Header("Infinite Mode Settings")]
    public bool infiniteModeEnabled = true;
    public GameObject treePrefab;
    public GameObject soldierPrefab;
    public GameObject playerPrefab;
    public GameObject medicTentPrefab;
    public int minSoldiers = 2;
    public int maxSoldiers = 5;
    public int numTrees = 20;
    public Text totalRescuedText;
    public Text winText;
    public Text gameOverText;
    public Camera mainCamera;
    public float minSpacing = 1.5f; // Minimum distance between objects

    private int totalRescued = 0;
    private int soldiersInLevel = 0;
    private List<GameObject> spawnedTrees = new List<GameObject>();
    private List<GameObject> spawnedSoldiers = new List<GameObject>();
    private GameObject playerInstance;
    private GameObject medicTentInstance;
    private List<Vector2> usedPositions = new List<Vector2>();

    void Start()
    {
        if (infiniteModeEnabled)
        {
            SetupLevel();
            UpdateRescuedUI();
            EnableInputActions();
        }
    }

    private void OnEnable()
    {
        EnableInputActions();
    }

    private void OnDisable()
    {
        DisableInputActions();
    }

    private void EnableInputActions()
    {
        if (resetGameAction != null && resetGameAction.action != null)
            resetGameAction.action.Enable();
        if (quitToMenuAction != null && quitToMenuAction.action != null)
            quitToMenuAction.action.Enable();
    }

    private void DisableInputActions()
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
        // ...existing code...

    void SetupLevel()
    {
        ClearLevel();
        soldiersInLevel = Random.Range(minSoldiers, maxSoldiers + 1);
        usedPositions.Clear();
        Vector2 playerPos = GetSafeRandomPosition(usedPositions, minSpacing);
        playerInstance = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        usedPositions.Add(playerPos);
        Vector2 tentPos = GetSafeRandomPosition(usedPositions, minSpacing);
        medicTentInstance = Instantiate(medicTentPrefab, tentPos, Quaternion.identity);
        usedPositions.Add(tentPos);
        playerManager = playerInstance.GetComponent<PlayerManager>();
        medicTentManager = medicTentInstance.GetComponent<MedicTentManager>();
        SpawnSoldiers(soldiersInLevel);
        SpawnTrees(numTrees);
        // TODO: Ensure path is completable
    }

    // --- GameManager-like functionality ---
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

    public void CheckWinCondition(GameUIManager uiManager)
    {
        if (GetRescuedSoldierCount() >= GetMaxRescuedSoldiers() && GetMaxRescuedSoldiers() > 0)
        {
            uiManager.ShowWin();
        }
    }

    void ClearLevel()
    {
        foreach (var t in spawnedTrees) Destroy(t);
        foreach (var s in spawnedSoldiers) Destroy(s);
        spawnedTrees.Clear();
        spawnedSoldiers.Clear();
        if (playerInstance) Destroy(playerInstance);
        if (medicTentInstance) Destroy(medicTentInstance);
    }

    void SpawnSoldiers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetSafeRandomPosition(usedPositions, minSpacing);
            var soldier = Instantiate(soldierPrefab, pos, Quaternion.identity);
            spawnedSoldiers.Add(soldier);
            usedPositions.Add(pos);
        }
    }

    void SpawnTrees(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GetSafeRandomPosition(usedPositions, minSpacing);
            var tree = Instantiate(treePrefab, pos, Quaternion.identity);
            spawnedTrees.Add(tree);
            usedPositions.Add(pos);
        }
    }

    Vector2 GetSafeRandomPosition(List<Vector2> usedPositions, float minDist)
    {
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));
        int attempts = 0;
        while (attempts < 100)
        {
            Vector2 pos = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            bool valid = true;
            foreach (var used in usedPositions)
            {
                if (Vector2.Distance(pos, used) < minDist)
                {
                    valid = false;
                    break;
                }
            }
            if (valid) return pos;
            attempts++;
        }
        // fallback
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
    public void OnSoldierRescued()
    {
        totalRescued++;
        soldiersInLevel--;
        UpdateRescuedUI();
        if (soldiersInLevel <= 0 && !isGameWon)
        {
            isGameWon = true;
            winText.text = "All soldiers rescued! Press any key for next mission.";
            winText.gameObject.SetActive(true);
            StartCoroutine(WaitForNextMission());
        }
    }

    void UpdateRescuedUI()
    {
        if (totalRescuedText)
            totalRescuedText.text = "Total Rescued: " + totalRescued;
        if (winText && isGameWon)
            winText.text = "All soldiers rescued! Press any key for next mission.";
        if (gameOverText && isGameOver)
            gameOverText.text = "Game Over! Total rescued: " + totalRescued;
    }

    System.Collections.IEnumerator WaitForNextMission()
    {
        while (!Input.anyKeyDown)
            yield return null;
        winText.gameObject.SetActive(false);
        isGameWon = false;
        SetupLevel();
    }

    public void OnGameOver()
    {
        isGameOver = true;
        gameOverText.text = "Game Over! Total rescued: " + totalRescued;
        gameOverText.gameObject.SetActive(true);
    }
}