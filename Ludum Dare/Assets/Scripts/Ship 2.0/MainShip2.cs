using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainShip2 : MonoBehaviour
{
    //Thrusters
    Rigidbody2D rb;
    const float torque = 0.02f;
    const float thrustForce = 8.0f;
    const float moveSpeed = 0.05f;
    [SerializeField] Vector2[] thrust = new Vector2[4];

    //Grapple Hook
    [SerializeField] GameObject grappleGun;
    [SerializeField] GrapplingRope grappleRope;
    float grappleAngle;
    float grappleAngleCurrent;
    float grappleAngleDelta;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grappleGun = GameObject.Find("GrapplePivot");
        grappleRope = FindObjectOfType<GrapplingRope>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        if (Input.GetKey(KeyCode.E))
        {
            //gameObject.transform.Rotate(new Vector3(0, 0, -0.2f));
            rb.AddTorque(-torque);
            rb.AddForce(new Vector2(moveSpeed, 0));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            //gameObject.transform.Rotate(new Vector3(0, 0, 0.2f));
            rb.AddTorque(torque);
            rb.AddForce(new Vector2(-moveSpeed, 0));
        }

        //Lock Ship to Grapple Hook
        if(grappleRope.enabled)
        {
            //The current difference between the grapple angle and the ship angle
            grappleAngleCurrent = Mathf.DeltaAngle(grappleGun.transform.eulerAngles.z, rb.rotation);
            
            //Get the difference between the starting angle difference and the current angle difference
            grappleAngleDelta = Mathf.DeltaAngle(grappleAngle, grappleAngleCurrent);

            //Change rotation accordingly
            rb.rotation -= grappleAngleDelta;
        }


        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(rb.transform.up * thrustForce * thrust[0].y);
            rb.AddForce(rb.transform.right * thrustForce * thrust[0].x);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(rb.transform.up * thrustForce * thrust[1].y);
            rb.AddForce(rb.transform.right * thrustForce * thrust[1].x);
            //fireSource.Play();
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(rb.transform.up * thrustForce * thrust[2].y);
            rb.AddForce(rb.transform.right * thrustForce * thrust[2].x);
            //fireSource.Stop();
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(rb.transform.up * thrustForce * thrust[3].y);
            rb.AddForce(rb.transform.right * thrustForce * thrust[3].x);
            //fireSource.Stop();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Testing");
        }
    }

    public void AddNewThrust(Vector2 newThrust, int side)
    {
        thrust[side] = newThrust;
    }

    public void LockGrappleGun()
    {
        //The angle that represents the difference between the grapple hook's rotation and the player's rotation
        grappleAngle = Mathf.DeltaAngle(grappleGun.transform.eulerAngles.z, rb.rotation);
    }
}
