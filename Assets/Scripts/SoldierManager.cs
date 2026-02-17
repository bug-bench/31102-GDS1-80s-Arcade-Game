using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager Instance { get; private set; }
    public GameObject[] carriedSoldiers = new GameObject[3];
    private int soldierCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool TryPickupSoldier(GameObject soldier)
    {
        if (soldierCount >= carriedSoldiers.Length)
            return false;

        carriedSoldiers[soldierCount] = soldier;
        soldierCount++;
        soldier.SetActive(false);
        return true;
    }

    public int GetCarriedCount()
    {
        return soldierCount;
    }

    public void ResetSoldiers()
    {
        for (int i = 0; i < carriedSoldiers.Length; i++)
        {
            carriedSoldiers[i] = null;
        }
        soldierCount = 0;
    }
}
