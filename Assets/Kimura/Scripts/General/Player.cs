using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    public bool canMove = true;

    void Update()
    {
        if (!canMove) return;
        float h = 0f;

        if (Keyboard.current.leftArrowKey.isPressed) h = -1;
        if (Keyboard.current.rightArrowKey.isPressed) h = 1;

        transform.Translate(h * Time.deltaTime * 5f, 0, 0);
    }

}
