using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster2 : Part
{
    //Forces
    float thrustForce = 8.0f;
    float angularAdjuster = 10.0f; //The number you divide the angular velocity by when offsetting torque

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
                    rb.AddForce(rb.transform.right * thrustForce);
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
                //rb.rotation = Mathf.LerpAngle(rb.rotation, shootDirection, 10.0f * Time.deltaTime);
                rb.AddForce(rb.transform.right * thrustForce * 1.0f);
                break;
        }
    }

    public override IEnumerator LaunchPart()
    {
        //Remove tether
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        line.enabled = false;
        state = State.Fire;
        player.PartStack[shipSide].Pop();
        Physics2D.IgnoreLayerCollision(8, 10, true); //Prevent Ship from colliding with thruster while firing

        //Remove Thruster from Stack
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(8, 10, false); //Return collision between ship and thruster

        //Return to Loose State
        yield return new WaitForSeconds(5.0f);
        state = State.Loose;
        particles.SetActive(false);
        line.enabled = true;
    }
}
