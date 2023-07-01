using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool CanClick => m_remainingCooldown <= 0;

    public Camera mainCamera;

    public float moveSpeed = 10;
    public float xRotationSensitivity = 4;
    public float yRotationSensitivity = 4;
    public float clickCooldown = 0.5f;

    public float successDelay = 0.1f;
    public float failDelay = 0.5f;
    private float m_remainingCooldown;

    [SerializeField] private Rigidbody m_rigidbody;
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError(GetType() + " already attached to " + gameObject.name + "!");
        else Instance = this;

        m_rigidbody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_remainingCooldown -= Time.fixedDeltaTime;
        m_remainingCooldown = Mathf.Max(0, m_remainingCooldown);

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
        float xRotation = Input.GetAxis("Mouse X");
        float yRotation = Input.GetAxis("Mouse Y");
        xRotation *= xRotationSensitivity * Time.deltaTime;
        yRotation *= yRotationSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, xRotation);
        var eul = mainCamera.transform.localEulerAngles;
        if (eul.x > 180) eul.x -= 360;
        eul.x += yRotation * -1;
        eul.x = Mathf.Clamp(eul.x, -90, 90);
        mainCamera.transform.localEulerAngles = eul;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && CanClick)
        {
            if (WaveGenerator.IsInSweetspot)
            {
                EventHandler.TriggerEvent("SweetspotHit");
                clickCooldown = successDelay;
            }
            else
            {
                EventHandler.TriggerEvent("SweetspotMissed");
                clickCooldown = failDelay;
            }
        }       
    }
}
