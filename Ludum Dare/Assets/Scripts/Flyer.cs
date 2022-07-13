using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Enemy
{
    //Variables
    GameObject player;
    Rigidbody2D rb;
    float timer;
    float shootTimer;
    const float force = 2.0f;
    [SerializeField] GameObject bullet;

    //State Machine
    public enum State
    {
        Roaming,
        Attacking
    }
    [SerializeField] protected State state = State.Roaming;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MainShip2>().gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
        timer = 10.0f;
        shootTimer = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case State.Roaming:
                //Switch State
                if(Vector2.Distance(transform.position, player.transform.position) < 30)
                {
                    state = State.Attacking;
                }
                //Change direction
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    ChangeDirection();
                }
                break;

            case State.Attacking:
                //Switch State
                if(Vector2.Distance(transform.position, player.transform.position) > 50)
                {
                    state = State.Roaming;
                }
                //Shoot
                shootTimer -= Time.deltaTime;
                if (shootTimer < 0)
                {
                    Shoot();
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Roaming:
                rb.AddForce(transform.right * force);
                break;

            case State.Attacking:
                //Rotate
                Vector2 direction = player.transform.position - transform.position;
                direction.Normalize();
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;

                //Move
                rb.AddForce(transform.right * force);
                break;
        }
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 360);
        rb.rotation = direction;
        timer = 10.0f;
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, rb.rotation)));
        shootTimer = 4.0f;
    }
}
