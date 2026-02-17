using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager Instance { get; private set; }
    public GameObject[] deployedSoldiers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        deployedSoldiers = GameObject.FindGameObjectsWithTag("Soldier");
    }

    public void RemoveFromDeployed(GameObject soldier)
    {
        for (int i = 0; i < deployedSoldiers.Length; i++)
        {
            if (deployedSoldiers[i] == soldier)
            {
                deployedSoldiers[i] = null;
                break;
            }
        }
    }
}
