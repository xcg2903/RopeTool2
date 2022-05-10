using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] bool attached;
    [SerializeField] float forceDirection;
    [SerializeField] GameObject bullet;
    LineRenderer line;
    AudioSource laserSound;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MainShip>().gameObject;
        line = gameObject.GetComponent<LineRenderer>();
        laserSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        forceDirection = Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z;

        if (attached)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                laserSound.Play();
                Debug.Log(transform.position);
            }

            line.SetPosition(0, transform.position);
            line.SetPosition(1, player.transform.position);
        }
        else
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            if (!attached)
            {
                DistanceJoint2D joint = gameObject.GetComponent<DistanceJoint2D>();
                joint.connectedBody = player.GetComponent<Rigidbody2D>();

                //TRIG STUFF
                float myRadius = gameObject.GetComponent<CircleCollider2D>().radius;
                float shipRadius = collision.gameObject.GetComponent<CircleCollider2D>().radius;

                //This code takes both circle colliders of the two colliding objects and attaches the joint at the point of collision
                float jointAngle = Mathf.Atan2(collision.gameObject.transform.position.y - gameObject.transform.position.y, collision.gameObject.transform.position.x - gameObject.transform.position.x);
                float jointAngleShip = Mathf.Atan2(gameObject.transform.position.y - collision.gameObject.transform.position.y, gameObject.transform.position.x - collision.gameObject.transform.position.x);
                float myContactAngle = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                float shipContactAngle = collision.gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                Debug.Log(shipContactAngle);

                joint.anchor = new Vector2(Mathf.Cos(jointAngle - myContactAngle) * myRadius, Mathf.Sin(jointAngle - myContactAngle) * myRadius);
                joint.connectedAnchor = new Vector2(Mathf.Cos(jointAngleShip - shipContactAngle) * shipRadius, Mathf.Sin(jointAngleShip - shipContactAngle) * shipRadius);

                attached = true;
            }
        }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        attached = false;
        StartCoroutine(JointAdd());
    }

    private IEnumerator JointAdd()
    {
        //Add a new joint shortly after the old one breaks
        yield return new WaitForSeconds(0.5f);
        gameObject.AddComponent<DistanceJoint2D>();
        DistanceJoint2D newjoint = gameObject.GetComponent<DistanceJoint2D>();
        newjoint.enableCollision = true;
        newjoint.maxDistanceOnly = true;
    }
}
