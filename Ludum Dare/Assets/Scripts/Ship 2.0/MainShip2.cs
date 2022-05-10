using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShip2 : MonoBehaviour
{
    Rigidbody2D rb;
    const float torque = 0.02f;
    const float moveSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //gameObject.transform.Rotate(new Vector3(0, 0, -0.2f));
            rb.AddTorque(-torque);
            rb.AddForce(new Vector2(moveSpeed, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            //gameObject.transform.Rotate(new Vector3(0, 0, 0.2f));
            rb.AddTorque(torque);
            rb.AddForce(new Vector2(-moveSpeed, 0));
        }
    }
}
