using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class System_snow : MonoBehaviour
{
    [SerializeField]
    public Image gaugeSlider;
    public Timer timer;
    int maxCount = 15;
    int currentCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gaugeSlider.fillAmount = 1f;
    }


    public void OnButtonClik()
    {
        if (currentCount >= maxCount) return;

        currentCount++;
        gaugeSlider.fillAmount =1f- (float)currentCount/maxCount;
        if (currentCount >= maxCount)
        {
            GameClear();
        }
    }
   void GameClear()
    {
        timer.GameClear();
    }
}
