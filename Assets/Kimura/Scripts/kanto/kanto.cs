using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class kanto : MonoBehaviour
{
    [SerializeField]
    public Timer_reverse timer;

    // Update is called once per frame
    void Update()
    {
        float z = transform.eulerAngles.z;
        if (z > 30 && z < 330)
        {
            timer.GameOver();
        }
    }
}
