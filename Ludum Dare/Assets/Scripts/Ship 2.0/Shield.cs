using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Part
{
    //Forces
    float thrustForce = 2.0f;
    float angularAdjuster = 10.0f; //The number you divide the angular velocity by when offsetting torque

    //Lightning
    [SerializeField] GameObject barrier; //Barrier preventing player from attaching the electric side of the shield to their ship when inactive
    [SerializeField] GameObject lightning; //Damage Hitbox
    Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();

        lightning.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Loose:
                break;
            case State.Attached:
                if (Input.GetKey(activeKey))
                {
                    animator.SetBool("active", true);
                    lightning.SetActive(true);
                    barrier.SetActive(false);
                }
                else
                {
                    animator.SetBool("active", false);
                    lightning.SetActive(false);
                    barrier.SetActive(true);
                }
                break;
            case State.Fire:
                break;
        }
    }

    void FixedUpdate()
    {
        //Thruster Forces
        switch (state)
        {
            case State.Loose:
                break;
            case State.Attached:
                break;
            case State.Fire:
                break;
        }
    }

    public override IEnumerator LaunchPart()
    {
        //Remove tether
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        Destroy(gameObject.GetComponent<MainShipCollisions>());
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        line.enabled = false;

        //Launch Shield
        state = State.Fire; 
        player.PartStack[shipSide].Pop(); //Remove from stack
        Destroy(gameObject.GetComponent<MainShipCollisions>()); //Delete Part Stacking
        Physics2D.IgnoreLayerCollision(8, 10, true); //Prevent Ship from colliding with thruster while firing
        Physics2D.IgnoreLayerCollision(10, 10, true); //Prevent thrusters from colliding with themselves while firing
        animator.SetBool("active", true); //Animate active
        lightning.SetActive(true); //Turn on hitbox
        barrier.SetActive(false); //Turn off box covering attack hitbox

        Vector2 launchForce = rb.transform.right * rb.velocity.magnitude * 2;
        if (launchForce.magnitude < 24)
        {
            rb.AddForce(-rb.transform.right * 24); //Launch with force of 24
        }
        else
        {
            rb.AddForce(-rb.transform.right * rb.velocity.magnitude * 2); //Velocity based Launch
        }

        //Remove Thruster from Stack
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(8, 10, false); //Return collision between ship and thruster
        Physics2D.IgnoreLayerCollision(10, 10, false); //Return collision between thruster and thruster

        //Return to Loose State
        yield return new WaitForSeconds(5.0f);
        state = State.Loose;
        line.enabled = true;
        animator.SetBool("active", false); //Animate deactive
        lightning.SetActive(false); //Turn off hitbox
        barrier.SetActive(true); //Turn on box covering attack hitbox
    }

    protected override IEnumerator KnockedOff()
    {
        StartCoroutine(base.KnockedOff());
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("active", false);
        lightning.SetActive(false);
        barrier.SetActive(true);
    }
}
