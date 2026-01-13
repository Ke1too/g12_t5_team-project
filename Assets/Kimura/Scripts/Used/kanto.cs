using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class kanto : MonoBehaviour
{
    [SerializeField]
    public Timer_reverse timer;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    void Start()
    {
        StartCoroutine(StartAfter());
    }
    IEnumerator StartAfter()
    {
        yield return new WaitForSeconds(3f);
        rb.simulated = true;
        yield return new WaitForSeconds(3f);
        rb.simulated = false;
    }

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
