using UnityEngine;

public class BendDrag2D : MonoBehaviour
{
    public Transform root;
    public Transform mid;
    public float maxAngle = 45f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDrag()
    {
        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        Vector2 dir = mouse - root.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        // ’†ŠÔ‚Ææ’[‚Å‹È‚ª‚è—Ê‚ğ•Ï‚¦‚é
        mid.localRotation = Quaternion.Euler(0, 0, angle * 0.5f);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void OnMouseUp()
    {
        // —£‚µ‚½‚çŒ³‚É–ß‚éi‰º•~‚«Š´j
        mid.localRotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
    }
}
