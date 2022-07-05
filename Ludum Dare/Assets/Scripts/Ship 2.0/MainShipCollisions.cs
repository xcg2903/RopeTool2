using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShipCollisions : MonoBehaviour
{
    MainShip2 playerShip;
    Part hitPart;

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
        if (collision.gameObject.GetComponent<Part>())
        {
            GameObject obj = collision.collider.gameObject;
            if (obj.tag == "Left" || obj.tag == "Right" || obj.tag == "Front")
            {
                hitPart = collision.gameObject.GetComponent<Part>();
                if (hitPart.ThrustState == Part.State.Loose)
                {
                    //Add Fixed Joint
                    if (!hitPart.gameObject.GetComponent<FixedJoint2D>())
                    {
                        hitPart.gameObject.AddComponent<FixedJoint2D>();
                        FixedJoint2D newjoint = hitPart.gameObject.GetComponent<FixedJoint2D>();
                        newjoint.enableCollision = true;
                    }

                    //Attach Fixed Joint
                    hitPart.AttachPart(gameObject.GetComponentInParent<Rigidbody2D>().gameObject);

                    if (gameObject.tag == "Front")
                    {
                        hitPart.AssignSide(0);
                        playerShip.PartStack[0].Push(hitPart);
                    }
                    if (gameObject.tag == "Back")
                    {
                        hitPart.AssignSide(1);
                        playerShip.PartStack[1].Push(hitPart);
                    }
                    if (gameObject.tag == "Right")
                    {
                        hitPart.AssignSide(2);
                        playerShip.PartStack[2].Push(hitPart);
                    }
                    if (gameObject.tag == "Left")
                    {
                        hitPart.AssignSide(3);
                        playerShip.PartStack[3].Push(hitPart);
                    }
                }
            }
        }
    }
}
