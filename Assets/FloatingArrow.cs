using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 10f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition; // 親(ArrowMarker)基準で動かす
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
