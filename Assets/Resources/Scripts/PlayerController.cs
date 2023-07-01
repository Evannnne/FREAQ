using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;

    public float moveSpeed = 10;
    public float xRotationSensitivity = 4;

    [SerializeField] private Rigidbody m_rigidbody;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        float xInput = 0;
        float yInput = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) xInput = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) xInput = 1;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) yInput = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) yInput = -1;

        Vector3 move = new Vector3(xInput, 0, yInput);
        move = mainCamera.transform.TransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);
        move = move.normalized;
        move *= moveSpeed;
        move *= Time.fixedDeltaTime;

        m_rigidbody.MovePosition(m_rigidbody.position + move);

        // Camera rotation
        float rotation = 0;
        rotation = Input.GetAxis("Mouse X");
        //if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftBracket)) rotation = -1;
        //if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightBracket)) rotation = 1;
        rotation *= xRotationSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);
    }
}
