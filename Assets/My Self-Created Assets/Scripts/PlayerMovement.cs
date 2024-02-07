using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform m_Cam;
    public GameObject m_Canvas;
    public float eyeLevel = 1.6f;
    public float normalHeight = 1.8f;
    public float crouchHeight = 0.8f;
    public float normalSpeed = 10f;
    public float crouchMultiplier = 0.8f;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float sprintMultiplier = 1.5f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sensitivity;
    public AudioSource audioDeath;
    float headRotation = 10f;
    private CapsuleCollider m_Capsule;
    private Rigidbody m_Rigidbody;

    private bool crouching;

    public bool busy = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_Capsule = GetComponent<CapsuleCollider>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Capsule.height = normalHeight;
        m_Cam.position = new Vector3(transform.position.x, transform.position.y - normalHeight / 2 + eyeLevel, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!busy)
        {
            Move();
            Look();
        }
    }

    void Move()
    { 

        // Handles crouching ------------------------------------------------------------------------------------------------------------------
        if (Input.GetKey(crouchKey))
        {
            // Crouch if crouchKey pressed and not already crouching
            if (!crouching)
            {
                crouching = true;
                m_Capsule.height = crouchHeight;
                Vector3 tempHeight = new Vector3(transform.position.x, transform.position.y - (normalHeight - crouchHeight) / 2, transform.position.z);
                transform.position = tempHeight;
                m_Cam.position = tempHeight;
            }

        }
        else if (crouching)
        {
            // Stand up if crouchKey not pressed and not already crouching and not stuck under something.
            var startPos = transform.position + new Vector3(0, crouchHeight - (normalHeight * 0.5f), 0);
            var length = (normalHeight - crouchHeight);

            if (!Physics.Raycast(startPos, Vector3.up, length))
            {
                crouching = false;
                m_Capsule.height = normalHeight;
                m_Cam.position = new Vector3(transform.position.x, transform.position.y - normalHeight / 2 + eyeLevel, transform.position.z);
            }

        }
        // Handles movement with WASD -----------------------------------------------------------------------------------------------------------------------------------------

        float speed = normalSpeed;
        if (crouching)
        {
            speed *= crouchMultiplier;
        }
        else if (Input.GetKey(sprintKey))
        {
            speed *= sprintMultiplier;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveBy = transform.right * x + transform.forward * z;
        m_Rigidbody.MovePosition(transform.position + moveBy.normalized * speed * Time.deltaTime);
    }

    // Handles looking around with the mouse ------------------------------------------------------------------------------------------------------
    void Look()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1f;

        transform.Rotate(0f, x, 0f);

        headRotation -= y;

        m_Cam.localEulerAngles = new Vector3( Mathf.Clamp(-headRotation, -70, 90), 0f, 0f);

        if (headRotation < -90)
        {
            headRotation = -90f;
        }
        else if (headRotation > 70)
        {
            headRotation = 70f;
        }
    }

    // Handles dying ---------------------------------------------------------------------------------------------------------
    public void OnDeath()
    {
        StartCoroutine(OnDeath2());
    }
    private IEnumerator OnDeath2()
    {
        busy = true;
        GameObject monster = GameObject.Find("Monster");
        transform.rotation = Quaternion.LookRotation(monster.transform.position - transform.position);
        transform.rotation = Quaternion.Euler(-20, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        GameObject.Find("HearingCanvas").SetActive(false);
        yield return new WaitForSeconds(2.5f);
        audioDeath.Play();
        m_Canvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
