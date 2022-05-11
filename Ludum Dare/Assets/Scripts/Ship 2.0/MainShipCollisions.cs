using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShipCollisions : MonoBehaviour
{
    MainShip2 playerShip;
    Thruster2 hitThruster;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = FindObjectOfType<MainShip2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Thruster2>())
        {
            hitThruster = collision.gameObject.GetComponent<Thruster2>();
            if (!hitThruster.attached)
            {
                hitThruster.AttachRocket(gameObject.GetComponentInParent<Rigidbody2D>().gameObject);

                if (gameObject.name == "Front")
                {
                    playerShip.AddNewThrust(new Vector2(0, -1));
                }
                if (gameObject.name == "Back")
                {
                    playerShip.AddNewThrust(new Vector2(0, 1));
                }
                if (gameObject.name == "Left")
                {
                    playerShip.AddNewThrust(new Vector2(1, 0));
                }
                if (gameObject.name == "Right")
                {
                    playerShip.AddNewThrust(new Vector2(-1, 0));
                }
            }
        }
    }
}
