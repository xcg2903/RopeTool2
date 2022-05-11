using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShip2 : MonoBehaviour
{
    Rigidbody2D rb;
    const float torque = 0.02f;
    const float thrustForce = 1.0f;
    const float moveSpeed = 0.05f;
    Vector2 thrust = new Vector2(0, 0);
    float forceDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        forceDirection = Mathf.Deg2Rad * rb.transform.rotation.eulerAngles.z;

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
        if (!Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * thrustForce * -thrust.x);
            //fireSource.Stop();
        }
    }

    public void AddNewThrust(Vector2 newThrust)
    {
        thrust += newThrust;
    }
}
