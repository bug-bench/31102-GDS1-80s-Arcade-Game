using UnityEngine;

public class MedicTentManager : MonoBehaviour
{
    public GameObject[] droppedOffSoldiers;
    private int droppedOffCount = 0;

    public bool TryDepositSoldier(GameObject soldier)
    {
        if (droppedOffSoldiers == null)
        {
            droppedOffSoldiers = new GameObject[GameObject.FindGameObjectsWithTag("Soldier").Length];
        }
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
