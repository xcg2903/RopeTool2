using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster2 : MonoBehaviour
{
    //Forces
    Rigidbody2D rb;
    Rigidbody2D rbPlayer;
    [SerializeField] float thrustForce = 10.0f;
    [SerializeField] float forceDirection;
    LineRenderer line;

    //State Machine
    public enum State
    {
        Loose,
        Attached,
        Fire
    }
    private State state = State.Loose;

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
        fireSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forceDirection = Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z;

        //Animate Thrusters
        switch(state)
        {
            case State.Loose:
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
                break;
            case State.Attached:
                if (Input.GetKey(activeKey))
                {
                    //fireSource.Play();
                    rb.AddForce(rb.transform.right * thrustForce);
                    rbPlayer.AddTorque(rbPlayer.angularVelocity / -20);
                    particles.SetActive(true);

                    if(Input.GetKey(KeyCode.Space))
                    {
                        shootDirection = rbPlayer.rotation;
                        Destroy(gameObject.GetComponent<FixedJoint2D>());
                        JointAdd();
                        state = State.Fire;
                    }
                }
                else
                {
                    //fireSource.Stop();
                    particles.SetActive(false);
                }

                line.SetPosition(0, transform.position);
                line.SetPosition(1, attachedTarget.transform.position);
                break;
            case State.Fire:
                rb.rotation = Mathf.LerpAngle(rb.rotation, shootDirection, 10.0f * Time.deltaTime);
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

    private void OnJointBreak2D(Joint2D joint)
    {
        state = State.Loose;
        particles.SetActive(false);
        StartCoroutine(JointAdd());
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
                player.ThrusterArray[0].Add(this);
                break;
            //Back
            case 1:
                activeKey = KeyCode.S;
                gameObject.tag = "Back";
                player.ThrusterArray[1].Add(this);
                break;
            //Right
            case 2:
                activeKey = KeyCode.D;
                gameObject.tag = "Right";
                player.ThrusterArray[2].Add(this);
                break;
            //Left
            case 3:
                activeKey = KeyCode.A;
                gameObject.tag = "Left";
                player.ThrusterArray[3].Add(this);
                break;
        }

        //Make this thruster able to collect more thrusters
        gameObject.AddComponent<MainShipCollisions>();
    }
}
