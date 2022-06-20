using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MainShip2>().gameObject;
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Shoot()
    {
        Vector3 playerPos = player.transform.position;
        float targetx = playerPos.x - transform.position.x;
        float targety = playerPos.y - transform.position.y;

        float angle = Mathf.Atan2(targety, targetx) * Mathf.Rad2Deg;

        Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(Shoot());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Thruster2>())
        {
            //If thruster is firing
            if (collision.gameObject.GetComponent<Thruster2>().ThrustState == Thruster2.State.Fire)
            {
                GotHit();
            }
        }
    }

    private void GotHit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject, 0.5f);

    }
}
