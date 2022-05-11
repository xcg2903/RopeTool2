using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShip2 : MonoBehaviour
{
    //Thrusters
    Rigidbody2D rb;
    const float torque = 0.02f;
    const float thrustForce = 5.0f;
    const float moveSpeed = 0.05f;
    [SerializeField] Vector2 thrust = new Vector2(0, 0);

    //Grapple Hook
    [SerializeField] GameObject grappleGun;
    [SerializeField] GrapplingRope grappleRope;
    float grappleAngle;
    float grappleAngleLast;
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
            /*
            if(grappleGun.transform.rotation.eulerAngles.z - rb.transform.rotation.eulerAngles.z > grappleAngle)
            {
                rb.AddTorque(0.1f);
            }
            if(grappleGun.transform.rotation.eulerAngles.z - rb.transform.rotation.eulerAngles.z < grappleAngle)
            {
                rb.AddTorque(-0.1f);
            }*/

            //rb.transform.localRotation.eulerAngles.z.Equals(Mathf.LerpAngle(rb.transform.localRotation.eulerAngles.z, grappleAngle, Time.deltaTime * 100));

            grappleAngleDelta = grappleGun.transform.eulerAngles.z - grappleAngleLast;
            rb.rotation += grappleAngleDelta;
            grappleAngleLast = grappleGun.transform.eulerAngles.z;
        }


        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustForce * thrust.y);
            //fireSource.Play();
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.up * thrustForce * -thrust.y);
            //fireSource.Play();
        }
        if (!Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * thrustForce * thrust.x);
            //fireSource.Stop();
        }
        if (!Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right * thrustForce * -thrust.x);
            //fireSource.Stop();
        }
    }

    public void AddNewThrust(Vector2 newThrust)
    {
        thrust += newThrust;
    }

    public void LockGrappleGun()
    {
        //grappleAngle = Mathf.DeltaAngle(grappleGun.transform.eulerAngles.z, rb.transform.localRotation.eulerAngles.z);
        //grappleAngle = Mathf.Repeat(grappleAngle + 180, 360) - 180;
        //Debug.Log(grappleAngle);
        grappleAngleLast = grappleGun.transform.eulerAngles.z;
    }
}
