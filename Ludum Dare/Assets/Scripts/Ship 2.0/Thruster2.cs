using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster2 : Part
{
    //Forces
    float thrustForce = 12.0f;
    float angularAdjuster = 10.0f; //The number you divide the angular velocity by when offsetting torque
    float velocityCurve;
    Vector2 currentForce;

    //Visuals
    GameObject particles;
    AudioSource fireSource;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        particles = GetComponentInChildren<ParticleSystem>().gameObject;
        particles.SetActive(false);
        fireSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        //Thruster Forces
        switch(state)
        {
            case State.Loose:
                break;
            case State.Attached:
                if (Input.GetKey(activeKey))
                {
                    //Add force in forward direction for this thruster
                    //fireSource.Play();

                    velocityCurve = (thrustForce) / rb.velocity.magnitude;
                    currentForce = rb.transform.right * thrustForce * velocityCurve;
                    
                    if(currentForce.magnitude > 32) //Ensure a force added to the ship is never greater than 32
                    {
                        rb.AddForce(rb.transform.right * 32);
                    }
                    else
                    {
                        rb.AddForce(rb.transform.right * thrustForce * velocityCurve);
                    }
                    rbPlayer.AddTorque(rbPlayer.angularVelocity / -angularAdjuster);

                    particles.SetActive(true);
                }
                else
                {
                    //fireSource.Stop();
                    particles.SetActive(false);
                }
                break;
            case State.Fire:
                //Shoot off in the direction the ship is facing
                //velocityCurve = (thrustForce * 2) / Mathf.Pow(rb.velocity.magnitude, 2);
                rb.AddForce(rb.transform.right * (thrustForce / 4));
                break;
        }
    }

    protected override IEnumerator LaunchPart()
    {
        //Remove tether
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        line.enabled = false;

        //Launch Rocket
        state = State.Fire;
        player.PartStack[shipSide].Pop(); //Remove from stack
        Destroy(gameObject.GetComponent<MainShipCollisions>()); //Delete Part Stacking
        Physics2D.IgnoreLayerCollision(8, 10, true); //Prevent Ship from colliding with thruster while firing
        Physics2D.IgnoreLayerCollision(10, 10, true); //Prevent thrusters from colliding with themselves while firing
        gameObject.tag = "PlayerAttack"; //Set hitboxes to damage
        Vector2 launchForce = rb.transform.right * rb.velocity.magnitude * 2;
        if(launchForce.magnitude < 32)
        {
            rb.AddForce(rb.transform.right * 32); //Launch with force of 32
        }
        else
        {
            rb.AddForce(rb.transform.right * rb.velocity.magnitude * 2); //Velocity based Launch
        }

        //Return collisions to normal
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(8, 10, false); //Return collision between ship and thruster
        Physics2D.IgnoreLayerCollision(10, 10, false); //Return collision between thruster and thruster

        //Return to Loose State
        yield return new WaitForSeconds(5.0f);
        state = State.Loose;
        gameObject.tag = "Untagged"; //Set hitboxes to normal
        particles.SetActive(false);
        line.enabled = true;
    }

    protected override IEnumerator KnockedOff()
    {
        StartCoroutine(base.KnockedOff());
        yield return new WaitForSeconds(0.5f);
        particles.SetActive(false);
    }
}
