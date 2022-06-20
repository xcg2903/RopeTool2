using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster2 : MonoBehaviour
{
    //Forces
    Rigidbody2D rb;
    Rigidbody2D rbPlayer;
    float thrustForce = 7.0f;
    LineRenderer line;

    //State Machine
    public enum State
    {
        Loose,
        Attached,
        Fire,
        None
    }
    [SerializeField] State state = State.Loose;

    //Visuals
    MainShip2 player;
    GameObject particles;
    GameObject attachedTarget;
    AudioSource fireSource;
    [SerializeField] KeyCode activeKey;

    //Fire Thruster
    int shipSide;
    float shootDirection;

    //Properties
    public State ThrustState
    {
        get { return state; }
        set { state = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rbPlayer = FindObjectOfType<MainShip2>().GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MainShip2>();
        particles = GetComponentInChildren<ParticleSystem>().gameObject;
        particles.SetActive(false);
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
        fireSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(activeKey) && Input.GetKeyDown(KeyCode.Space))
        {
            if(state == State.Attached)
            {
                //Check if the most recent addition to the list is this thruster
                int count = player.ThrusterStack[shipSide].Count;
                Debug.Log(player.ThrusterStack[shipSide].Peek());

                if (player.ThrusterStack[shipSide].Peek() == this)
                {
                    //Shoot off
                    StartCoroutine(FireThruster());
                }
            }
        }

        if(Input.GetKey(KeyCode.Y))
        {
            StartCoroutine(KnockedOff());
        }
    }

    void FixedUpdate()
    {
        //Animate Thrusters
        switch(state)
        {
            case State.Loose:
                //No rope visible
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
                particles.SetActive(false);
                break;
            case State.Attached:
                if (Input.GetKey(activeKey))
                {
                    //Add force in forward direction for this thruster
                    //fireSource.Play();
                    rb.AddForce(rb.transform.right * thrustForce);
                    rbPlayer.AddTorque(rbPlayer.angularVelocity / -20);
                    particles.SetActive(true);
                }
                else
                {
                    //fireSource.Stop();
                    particles.SetActive(false);
                }

                //Attach line to center of attached target
                line.SetPosition(0, transform.position);
                line.SetPosition(1, attachedTarget.transform.position);
                break;
            case State.Fire:
                //Shoot off in the direction the ship is facing
                //rb.rotation = Mathf.LerpAngle(rb.rotation, shootDirection, 10.0f * Time.deltaTime);
                rb.AddForce(rb.transform.right * thrustForce);
                break;
        }
    }

    public void AttachRocket(GameObject collision)
    {
        if (state == State.Loose)
        {
            FixedJoint2D joint = gameObject.GetComponent<FixedJoint2D>();
            joint.connectedBody = collision.GetComponent<Rigidbody2D>();

            //TRIG STUFF
            float myRadius = gameObject.GetComponent<CircleCollider2D>().radius;
            float shipRadius = collision.GetComponent<CircleCollider2D>().radius;

            //This code takes both circle colliders of the two colliding objects and attaches the joint at the point of collision
            float jointAngle = Mathf.Atan2(collision.transform.position.y - gameObject.transform.position.y, collision.transform.position.x - gameObject.transform.position.x);
            float jointAngleShip = Mathf.Atan2(gameObject.transform.position.y - collision.transform.position.y, gameObject.transform.position.x - collision.transform.position.x);
            float myContactAngle = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            float shipContactAngle = collision.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            //Debug.Log(shipContactAngle);

            joint.anchor = new Vector2(Mathf.Cos(jointAngle - myContactAngle) * myRadius, Mathf.Sin(jointAngle - myContactAngle) * myRadius);
            joint.connectedAnchor = new Vector2(Mathf.Cos(jointAngleShip - shipContactAngle) * shipRadius, Mathf.Sin(jointAngleShip - shipContactAngle) * shipRadius);

            attachedTarget = collision;
            state = State.Attached;
        }
    }

    /*
    private void OnJointBreak2D(Joint2D joint)
    {
        state = State.Loose;
        particles.SetActive(false);
        StartCoroutine(JointAdd());
    }
    */

    private IEnumerator FireThruster()
    {
        //Shoot off
        shootDirection = rbPlayer.rotation;
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        line.enabled = false;
        state = State.Fire;

        //Remove Thruster from Stack
        yield return new WaitForSeconds(0.5f);
        player.ThrusterStack[shipSide].Pop();
        //StartCoroutine(JointAdd());

        //Return to Loose State
        yield return new WaitForSeconds(5.0f);
        state = State.Loose;
        line.enabled = true;
    }


    public void CallKnockOff()
    {
        //Call KnockedOff from another script
        StartCoroutine(KnockedOff());
    }
    private IEnumerator KnockedOff()
    {
        //Knock off
        Vector3 playerPos = player.gameObject.transform.position;
        float directx = transform.position.x - playerPos.x;
        float directy = transform.position.y - playerPos.y;
        rb.AddForce(new Vector2(directx, directy) * 30.0f);
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        line.enabled = false;
        state = State.None;

        //Remove Thruster from Stack, Return to Loose State
        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(JointAdd());
        state = State.Loose;
        line.enabled = true;
    }

    private IEnumerator JointAdd()
    {
        //Add a new joint shortly after the old one breaks
        yield return new WaitForSeconds(0.5f);
        gameObject.AddComponent<FixedJoint2D>();
        FixedJoint2D newjoint = gameObject.GetComponent<FixedJoint2D>();
        newjoint.enableCollision = true;
        //newjoint.maxDistanceOnly = true;
    }

    public void AssignSide(int side)
    {
        shipSide = side;

        //Assign Animation Activation Key
        //Assign tag so more thrusters can be added
        switch(side)
        {
            //Front
            case 0:
                activeKey = KeyCode.W;
                gameObject.tag = "Front";
                player.ThrusterStack[0].Push(this);
                break;
            //Back
            case 1:
                activeKey = KeyCode.S;
                gameObject.tag = "Back";
                player.ThrusterStack[1].Push(this);
                break;
            //Right
            case 2:
                activeKey = KeyCode.D;
                gameObject.tag = "Right";
                player.ThrusterStack[2].Push(this);
                break;
            //Left
            case 3:
                activeKey = KeyCode.A;
                gameObject.tag = "Left";
                player.ThrusterStack[3].Push(this);
                break;
        }

        //Make this thruster able to collect more thrusters
        gameObject.AddComponent<MainShipCollisions>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<TestEnemyBullet>())
        {
            player.LooseRockets();
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
