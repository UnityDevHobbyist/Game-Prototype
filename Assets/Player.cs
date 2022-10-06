using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    [SerializeField]private float m_Speed = 5f;
    Vector3 newRotation = new Vector3(0, 0, 0);
    bool crouching = false;
    bool isGrounded = false;

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
        else
        {

        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //Detect when the C arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.C))
        {
            newRotation = new Vector3(-90, 0, 0);
            transform.eulerAngles = newRotation;
            crouching = true;
        }
            

        //Detect when the C arrow key has been released
        if (Input.GetKeyUp(KeyCode.C))
        {
            newRotation = new Vector3(0, 0, 0);
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
    }

    void FixedUpdate()
    {
        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
    }
}