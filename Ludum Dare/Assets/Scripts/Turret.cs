using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    //Variables
    [SerializeField] GameObject gun;
    GameObject player;
    [SerializeField] GameObject bullet;
    Vector2 direction;
    float shootTimer = 1.0f;
    float angle;

    //Stat Machine
    public enum State
    {
        Off,
        Attacking
    }
    [SerializeField] protected State state = State.Off;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MainShip2>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Off:
                //Swtich State
                if (Vector2.Distance(transform.position, player.transform.position) < 30)
                {
                    StartFire();
                }
                break;
            case State.Attacking:
                //Switch State
                if (Vector2.Distance(transform.position, player.transform.position) > 30)
                {
                    state = State.Off;
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

    void StartFire()
    {
        //Set direction
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.transform.localEulerAngles = new Vector3(0, 0, angle);

        state = State.Attacking;
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position + (gun.transform.right) + (gun.transform.up * 0.5f), Quaternion.Euler(new Vector3(0, 0, angle)));
        Instantiate(bullet, transform.position + (gun.transform.right) + (-gun.transform.up * 0.5f), Quaternion.Euler(new Vector3(0, 0, angle)));
        shootTimer = 0.25f;
    }
}
