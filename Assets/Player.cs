using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    [SerializeField]private float m_Speed = 5f;
    Vector3 newRotation;
    bool crouching = false;
    bool isGrounded = false;
    int turn = 0;
    public Camera cam;
    public MeshRenderer playerCrouchrenderer;
    public MeshRenderer playerRenderer;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.transform.gameObject.CompareTag("Tree Trunk"))
            {
                //Debug.Log("Hit Tree Trunk");
                m_Rigidbody.velocity = new Vector3(0, 5, 0);
            }
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //Detect when the C arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.C))
        {
            newRotation = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
            crouching = true;
        }
            

        //Detect when the C arrow key has been released
        if (Input.GetKeyUp(KeyCode.C))
        {
            newRotation = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
            crouching = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!crouching && isGrounded)
            {
               //Apply a force to this Rigidbody in direction of this GameObjects up axis
               m_Rigidbody.AddForce(transform.up * 300, ForceMode.Acceleration);
            }
        }

        if (crouching)
        {
            m_Speed = 2.5f;
        }
        else
        {
            m_Speed = 5f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !crouching)
        {
            m_Speed = 7.5f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            turn = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn = 1;
        }
        else
        {
            turn = 0;
        }
    }

    void MovePlayer()
    {
        //Store user input as a movement vector
        Vector3 m_Input = transform.forward * Input.GetAxis("Vertical");

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);

        cam.transform.position = transform.position - transform.forward * 10;
    }

    void RotatePlayer()
    {
        transform.RotateAround(transform.position, transform.up, 100 * turn * Time.deltaTime);
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();

        if (crouching)
        {
            playerCrouchrenderer.enabled = true;
            playerRenderer.enabled = false;
        }
        else
        {
            playerCrouchrenderer.enabled = false;
            playerRenderer.enabled = true;
        }
    }
}