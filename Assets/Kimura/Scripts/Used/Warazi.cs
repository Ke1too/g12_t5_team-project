using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Warazi : MonoBehaviour
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
        yield return new WaitForSeconds(4f);
        rb.simulated = true;
        yield return new WaitForSeconds(3f);
        rb.simulated = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            timer?.GameOver();
        }
    }
}
