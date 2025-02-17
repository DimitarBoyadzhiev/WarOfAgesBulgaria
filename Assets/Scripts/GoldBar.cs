using UnityEngine;
using UnityEngine.UI;

public class GoldBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxGold(int goldMax)
    {
        slider.maxValue = goldMax;
    }

    public void SetGold(int gold)
    {
        slider.value = gold;
    }
}
