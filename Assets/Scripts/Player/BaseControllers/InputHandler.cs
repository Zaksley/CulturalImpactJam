using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    public bool JumpButton { get; private set; }

    public bool DialogueButton { get; private set; }
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        MovementInputVector = new Vector2(h, v);

        MousePosition = Input.mousePosition;

        JumpButton = Input.GetKeyDown(KeyCode.Space);

        DialogueButton = Input.GetKeyDown(KeyCode.E);
    }
}