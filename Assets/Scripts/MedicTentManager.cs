using UnityEngine;

public class MedicTentManager : MonoBehaviour
{
    public GameObject[] droppedOffSoldiers;
    private int droppedOffCount = 0;

    private void Start()
    {
        int soldierCount = GameObject.FindGameObjectsWithTag("Soldier").Length;
        droppedOffSoldiers = new GameObject[soldierCount];
    }

    public bool TryDepositSoldier(GameObject soldier)
    {
        if (droppedOffCount >= droppedOffSoldiers.Length)
            return false;

        droppedOffSoldiers[droppedOffCount] = soldier;
        droppedOffCount++;
        if (droppedOffCount == droppedOffSoldiers.Length)
        {
            Debug.Log("all soldiers dropped off");
        }
        return true;
    }

    public int GetDepositCount()
    {
        return droppedOffCount;
    }
}
