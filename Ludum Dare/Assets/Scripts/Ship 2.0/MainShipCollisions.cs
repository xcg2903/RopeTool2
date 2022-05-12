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
        if (collision.gameObject.GetComponent<Thruster2>() && collision.collider.gameObject.tag != "Back")
        {
            hitThruster = collision.gameObject.GetComponent<Thruster2>();
            if (!hitThruster.attached)
            {
                hitThruster.AttachRocket(gameObject.GetComponentInParent<Rigidbody2D>().gameObject);

                if (gameObject.tag == "Front")
                {
                    if (collision.collider.gameObject.tag == "Front")
                    {
                        playerShip.AddNewThrust(new Vector2(0, -1), 0);
                        hitThruster.AssignSide(0);
                    }
                    if (collision.collider.gameObject.tag == "Right")
                    {
                        playerShip.AddNewThrust(new Vector2(1, 0), 0);
                        hitThruster.AssignSide(0);
                    }
                    if (collision.collider.gameObject.tag == "Left")
                    {
                        playerShip.AddNewThrust(new Vector2(-1, 0), 0);
                        hitThruster.AssignSide(0);
                    }        
                }
                if (gameObject.tag == "Back")
                {
                    if (collision.collider.gameObject.tag == "Front")
                    {
                        playerShip.AddNewThrust(new Vector2(0, 1), 1);
                        hitThruster.AssignSide(1);
                    }
                    if (collision.collider.gameObject.tag == "Right")
                    {
                        playerShip.AddNewThrust(new Vector2(-1, 0), 1);
                        hitThruster.AssignSide(1);
                    }
                    if (collision.collider.gameObject.tag == "Left")
                    {
                        playerShip.AddNewThrust(new Vector2(1, 0), 1);
                        hitThruster.AssignSide(1);
                    }
                }
                if (gameObject.tag == "Right")
                {
                    if (collision.collider.gameObject.tag == "Front")
                    {
                        playerShip.AddNewThrust(new Vector2(-1, 0), 2);
                        hitThruster.AssignSide(2);
                    }
                    if (collision.collider.gameObject.tag == "Right")
                    {
                        playerShip.AddNewThrust(new Vector2(0, -1), 2);
                        hitThruster.AssignSide(2);
                    }
                    if (collision.collider.gameObject.tag == "Left")
                    {
                        playerShip.AddNewThrust(new Vector2(0, 1), 2);
                        hitThruster.AssignSide(2);
                    }
                }
                if (gameObject.tag == "Left")
                {
                    if (collision.collider.gameObject.tag == "Front")
                    {
                        playerShip.AddNewThrust(new Vector2(1, 0), 3);
                        hitThruster.AssignSide(3);
                    }
                    if (collision.collider.gameObject.tag == "Right")
                    {
                        playerShip.AddNewThrust(new Vector2(0, 1), 3);
                        hitThruster.AssignSide(3);
                    }
                    if (collision.collider.gameObject.tag == "Left")
                    {
                        playerShip.AddNewThrust(new Vector2(0, -1), 3);
                        hitThruster.AssignSide(3);
                    }
                }
            }
        }
    }
}
