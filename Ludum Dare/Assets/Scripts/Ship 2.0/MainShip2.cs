using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainShip2 : MonoBehaviour
{
    //Thrusters
    Rigidbody2D rb;
    [SerializeField] Stack<Thruster2>[] thrusterStack = new Stack<Thruster2>[4];

    //Grapple Hook
    [SerializeField] GameObject grappleGun;
    [SerializeField] GrapplingRope grappleRope;
    float grappleAngle;
    float grappleAngleCurrent;
    float grappleAngleDelta;

    //Properties
    public Stack<Thruster2>[] ThrusterStack
    {
        get { return thrusterStack; }
        set { thrusterStack = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grappleGun = GameObject.Find("GrapplePivot");
        grappleRope = FindObjectOfType<GrapplingRope>();

        //Thruster lists
        thrusterStack[0] = new Stack<Thruster2>();
        thrusterStack[1] = new Stack<Thruster2>();
        thrusterStack[2] = new Stack<Thruster2>();
        thrusterStack[3] = new Stack<Thruster2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Testing");
        }
    }

    void FixedUpdate()
    {
        //Lock Ship to Grapple Hook
        if(grappleRope.enabled)
        {
            //The current difference between the grapple angle and the ship angle
            grappleAngleCurrent = Mathf.DeltaAngle(grappleGun.transform.eulerAngles.z, rb.rotation);
            
            //Get the difference between the starting angle difference and the current angle difference
            grappleAngleDelta = Mathf.DeltaAngle(grappleAngle, grappleAngleCurrent);

            //Change rotation accordingly
            rb.rotation -= grappleAngleDelta;
        }
    }

    public void LockGrappleGun()
    {
        //The angle that represents the difference between the grapple hook's rotation and the player's rotation
        grappleAngle = Mathf.DeltaAngle(grappleGun.transform.eulerAngles.z, rb.rotation);
    }

    public void LooseRockets()
    {
        //Loop through all four stacks and pop all thrusters
        for (int i = 0; i < 4; i++)
        {
            bool looping = true;
            while (looping)
            {
                //Check if this side has no thrusters on it
                if (thrusterStack[i].Count == 0)
                {
                    Debug.Log(i + "  " + thrusterStack[i].Count);
                    looping = false;
                    break;
                }

                //Remove thruster from ship
                Thruster2 thruster = thrusterStack[i].Peek();
                thruster.CallKnockOff();
                thrusterStack[i].Pop();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TestEnemyBullet>())
        {
            LooseRockets();
            Destroy(collision.gameObject, 0.5f);
        }
    }

    //Old

    /*
    public void AddNewThrust(Vector2 newThrust, int side)
    {
        thrust[side] += newThrust;
    }
    */
}
