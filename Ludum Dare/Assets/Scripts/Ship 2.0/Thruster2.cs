using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster2 : MonoBehaviour
{
    Rigidbody2D rb;
    const float thrustForce = 1.0f;

    [SerializeField] public bool attached;
    [SerializeField] float forceDirection;
    LineRenderer line;

    GameObject player;
    GameObject particles;
    GameObject attachedTarget;
    AudioSource fireSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MainShip2>().gameObject;
        particles = GetComponentInChildren<ParticleSystem>().gameObject;
        particles.SetActive(false);
        line = gameObject.GetComponent<LineRenderer>();
        fireSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        forceDirection = Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z;

        //Apply thrust if attached to ship
        if (attached)
        {
            /*
            if (attachedTarget.tag == "Player")
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(new Vector2(Mathf.Cos(forceDirection), Mathf.Sin(forceDirection)) * thrustForce);
                    fireSource.Play();
                    particles.SetActive(true);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(new Vector2(Mathf.Cos(forceDirection), Mathf.Sin(forceDirection)) * -thrustForce);
                    fireSource.Play();
                    particles.SetActive(false);
                }
                if (!Input.GetKey(KeyCode.W))
                {
                    fireSource.Stop();
                    particles.SetActive(false);
                }

                line.SetPosition(0, transform.position);
                line.SetPosition(1, attachedTarget.transform.position);
            }
            else
            {
                if (attachedTarget.GetComponent<AIScript>().firingEngines)
                {
                    rb.AddForce(new Vector2(Mathf.Cos(forceDirection), Mathf.Sin(forceDirection)) * thrustForce / 2);
                    fireSource.Play();
                    particles.SetActive(true);
                }
                else
                {
                    fireSource.Stop();
                    particles.SetActive(false);
                }
                line.SetPosition(0, transform.position);
                line.SetPosition(1, attachedTarget.transform.position);
            }
            */
        }
        else
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
    }

    public void AttachRocket(GameObject collision)
    {
        if (!attached)
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
            attached = true;
        }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        attached = false;
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
}
