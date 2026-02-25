using UnityEngine;
using UnityEngine.UI;

public class InfiniteModeUI : MonoBehaviour
{
    public Text totalRescuedText;

    public void SetTotalRescued(int count)
    {
        if (totalRescuedText)
            totalRescuedText.text = "Total Rescued: " + count;
    }
}
