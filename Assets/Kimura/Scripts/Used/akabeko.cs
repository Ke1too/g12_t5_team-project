using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class akabeko : MonoBehaviour
{
    public Timer timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnButtonClick()
    {
        timer.GameClear();
    }
}
