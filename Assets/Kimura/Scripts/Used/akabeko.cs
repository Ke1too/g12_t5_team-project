using UnityEngine;
using System.Collections;

public class akabeko : MonoBehaviour
{
    public float maxAngle = 20f;
    public float shakeDuration = 0.4f;

    float currentAngle = 0f;
    bool isShaking = false;
    public Timer timer;
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnButtonClick()
    {
        timer.GameClear();
        StartCoroutine(ShakeSmooth());
    }
    IEnumerator ShakeSmooth()
    {
        isShaking = true;

        int loops = 2; // Åö ÉtÉã2âÒ

        for (int i = 0; i < loops; i++)
        {
            yield return RotateTo(maxAngle);
            yield return RotateTo(-maxAngle);
        }

        yield return RotateTo(0f); // å≥Ç…ñﬂÇ∑
        isShaking = false;
    }

    IEnumerator RotateTo(float target)
    {
        float start = currentAngle;
        float time = 0f;

        while (time < shakeDuration)
        {
            time += Time.deltaTime;
            float t = time / shakeDuration;
            currentAngle = Mathf.Lerp(start, target, t);
            yield return null;
        }

        currentAngle = target;
    }

}
