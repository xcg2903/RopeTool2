using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    //Forces
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Rigidbody2D rbPlayer;
    [SerializeField] protected LineRenderer line;

    //State Machine
    public enum State
    {
        Loose,
        Attached,
        Fire,
        None
    }
    [SerializeField] protected State state = State.Loose;

    //Visuals
    protected MainShip2 player;
    protected GameObject attachedTarget;
    [SerializeField] protected KeyCode activeKey;
    [SerializeField] protected GameObject sparks;

    //Fire Thruster
    protected int shipSide;

    //Properties
    public State ThrustState
    {
        get { return state; }
        set { state = value; }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rbPlayer = FindObjectOfType<MainShip2>().GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MainShip2>();
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = true;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        switch (state)
        {
            case State.Loose:
                //No rope visible
                //line.SetPosition(0, Vector3.zero);
                //line.SetPosition(1, Vector3.zero);
                //particles.SetActive(false);
                break;
            case State.Attached:
                //Attach line to center of attached target
                line.SetPosition(0, transform.position);
                line.SetPosition(1, attachedTarget.transform.position);

                //Get Keyboard Input
                if (Input.GetKey(activeKey) && Input.GetKeyDown(KeyCode.Space))
                {
                    //Check if the most recent addition to the list is this thruster
                    int count = player.PartStack[shipSide].Count;

                    if (player.PartStack[shipSide].Peek() == this)
                    {
                        //Shoot off
                        StartCoroutine(LaunchPart());
                    }
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    StartCoroutine(KnockedOff());
                }

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

                //ATTACHED BEHAVIOR IS DETERMINED BY CHILD CLASS

                break;
            case State.Fire:

                //FIRE BEHAVIOR IS DETERMINED BY CHILD CLASS

                break;
        }
    }

    public void AttachPart(GameObject collision)
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

    protected virtual IEnumerator LaunchPart()
    {
        //FIRE OFF PART BEHAVIOR
        //OVERRIDE THIS
        yield return new WaitForSeconds(1.0f);
        Debug.Log("DID NOT OVERRIDE");
    }


    public void CallKnockOff()
    {
        //Call KnockedOff from another script
        StartCoroutine(KnockedOff());
    }
    protected virtual IEnumerator KnockedOff()
    {

        //Apply force in opposite direction of player
        Vector3 playerPos = player.gameObject.transform.position;
        float directx = transform.position.x - playerPos.x;
        float directy = transform.position.y - playerPos.y;
        rb.AddForce(new Vector2(directx, directy) * 30.0f);

        //Remove tether
        Destroy(gameObject.GetComponent<FixedJoint2D>());
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        line.enabled = false;
        state = State.None;

        //Remove Thruster from Stack, Return to Loose State
        yield return new WaitForSeconds(0.5f);
        state = State.Loose;
        line.enabled = true;
    }

    public void AssignSide(int side)
    {
        shipSide = side;

        //Assign Animation Activation Key
        //Assign tag so more thrusters can be added
        switch (side)
        {
            //Front
            case 0:
                activeKey = KeyCode.W;
                gameObject.tag = "Front";
                break;
            //Back
            case 1:
                activeKey = KeyCode.S;
                gameObject.tag = "Back";
                break;
            //Right
            case 2:
                activeKey = KeyCode.D;
                gameObject.tag = "Right";
                break;
            //Left
            case 3:
                activeKey = KeyCode.A;
                gameObject.tag = "Left";
                break;
        }

        //Make this thruster able to collect more thrusters
        gameObject.AddComponent<MainShipCollisions>();
    }

    public void SpawnSparks(Vector2 hitPoint)
    {
        //Rotate Towards towards player
        Vector2 playerPos = player.transform.position;
        float targetx = playerPos.x - hitPoint.x;
        float targety = playerPos.y - hitPoint.y;

        float angle = Mathf.Atan2(targety, targetx) * Mathf.Rad2Deg;

        Instantiate(sparks, hitPoint, Quaternion.Euler(new Vector3(0, 0, angle)));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TestEnemyBullet>())
        {
            player.LooseThrusters();
            Destroy(collision.gameObject, 0.5f);
        }

        if (!collision.gameObject.GetComponentInParent<MainShip2>()) //Generate thrusts except for when hitting player
        {
            ContactPoint2D point = collision.GetContact(0);
            Vector2 pos = new Vector2(point.point.x, point.point.y);
            SpawnSparks(pos);
        }
    }

    //OLD
    /*
    private IEnumerator JointAdd()
    {
        //Add a new joint shortly after the old one breaks
        yield return new WaitForSeconds(0.5f);
        gameObject.AddComponent<FixedJoint2D>();
        FixedJoint2D newjoint = gameObject.GetComponent<FixedJoint2D>();
        newjoint.enableCollision = true;
        //newjoint.maxDistanceOnly = true;
    }
    */
}
