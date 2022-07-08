using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
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
        //Switch State
        if(state == State.Roaming && Vector2.Distance(transform.position, player.transform.position) < 30)
        {
            state = State.Attacking;
        }
        if (state == State.Attacking && Vector2.Distance(transform.position, player.transform.position) > 50)
        {
            state = State.Roaming;
        }

        switch(state)
        {
            case State.Roaming:
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    ChangeDirection();
                }
                break;

            case State.Attacking:
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
        //Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, rb.rotation)));
        shootTimer = 3.0f;
    }

    private void GotHit()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        Destroy(gameObject, 0.5f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).collider.gameObject.tag == "PlayerAttack")
        {
            GotHit();
        }
    }
}
