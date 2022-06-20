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
            if (hitThruster.ThrustState == Thruster2.State.Loose)
            {

                //Add Fixed Joint
                if (!hitThruster.gameObject.GetComponent<FixedJoint2D>())
                {
                    hitThruster.gameObject.AddComponent<FixedJoint2D>();
                    FixedJoint2D newjoint = hitThruster.gameObject.GetComponent<FixedJoint2D>();
                    newjoint.enableCollision = true;
                }

                //Attach Fixed Joint
                hitThruster.AttachRocket(gameObject.GetComponentInParent<Rigidbody2D>().gameObject);

                if (gameObject.tag == "Front")
                {
                    /*
                    if (collision.collider.gameObject.tag == "Front")
                    {
                        hitThruster.AssignSide(0);
                    }
                    if (collision.collider.gameObject.tag == "Right")
                    {
                        hitThruster.AssignSide(0);
                    }
                    if (collision.collider.gameObject.tag == "Left")
                    {
                        hitThruster.AssignSide(0);
                    }     
                    */
                    hitThruster.AssignSide(0);
                }
                if (gameObject.tag == "Back")
                {
                    hitThruster.AssignSide(1);
                }
                if (gameObject.tag == "Right")
                {
                    hitThruster.AssignSide(2);
                }
                if (gameObject.tag == "Left")
                {
                    hitThruster.AssignSide(3);
                }
            }
        }
    }
}
