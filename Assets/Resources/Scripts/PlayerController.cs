using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    None = 0,
    Pistol = 1
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool CanClick => m_remainingCooldown <= 0;
    public bool Failed => !CanClick && m_wasFail;

    public Camera mainCamera;

    public float moveSpeed = 10;
    public float xRotationSensitivity = 4;
    public float yRotationSensitivity = 4;

    public float successDelay = 0.1f;
    public float failDelay = 0.5f;

    public float health = 100;

    public float invincibilityPeriod = 2.0f;

    private float m_remainingCooldown;
    private bool m_wasFail = false;

    public Weapon currentWeapon;

    public GameObject projectilePrefab_pistol;

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_armRotator;

    [SerializeField] private Transform m_projectileOrigin_pistol;

    private Vector3 m_currentSway;

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
        if (GameManager.Instance.CurrentGameState != GameState.InProgress) return;

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
        m_animator.SetBool("Moving", move != Vector3.zero);

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
    private void LateUpdate()
    {
        if (GameManager.Instance.CurrentGameState != GameState.InProgress) return;

        float xRotation = Input.GetAxis("Mouse X");
        float yRotation = Input.GetAxis("Mouse Y");

        Vector3 targetSway = new Vector3(yRotation * -4, xRotation * 4, 0);
        m_currentSway = Vector3.Lerp(m_currentSway, targetSway, Time.deltaTime * 4);
        m_currentSway = new Vector3(Mathf.Clamp(m_currentSway.x, -10, 10), Mathf.Clamp(m_currentSway.y, -10, 10), 0);

        var eul = m_armRotator.transform.localEulerAngles;
        eul += m_currentSway;
        m_armRotator.transform.localEulerAngles = eul;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.InProgress) return;

        if (Input.GetMouseButtonDown(0) && CanClick)
        {
            if (WaveGenerator.IsInSweetspot)
            {
                EventHandler.TriggerEvent("SweetspotHit");
                m_remainingCooldown = successDelay;
                m_wasFail = false;
                HandleFire();
            }
            else
            {
                EventHandler.TriggerEvent("SweetspotMissed");
                m_remainingCooldown = failDelay;
                m_wasFail = true;
            }
        }       
    }

    private void HandleFire()
    {
        m_animator.SetTrigger("Fire");
        if(currentWeapon == Weapon.Pistol)
        {
            var dir = (mainCamera.transform.position + mainCamera.transform.forward * 100 - m_projectileOrigin_pistol.transform.position).normalized;
            var proj = Instantiate(projectilePrefab_pistol);
            proj.transform.position = m_projectileOrigin_pistol.position;
            proj.transform.forward = dir;
            proj.GetComponent<Rigidbody>().velocity = dir * 30;
        }
    }

    private float m_lastHitTime;

    public void OnHit(object hit)
    {
        if (Time.time - m_lastHitTime >= invincibilityPeriod)
        {
            EventHandler.TriggerEvent("PlayerDamaged");
            HitData data = (HitData)hit;
            health -= data.damage;
            m_lastHitTime = Time.time;

            if(health <= 0)
            {
                EventHandler.TriggerEvent("PlayerDeath");
            }
        }

    }
}
