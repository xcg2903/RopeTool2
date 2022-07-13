using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBullet : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3.0f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(gameObject.transform.right * 200.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        //if (collision.GetContact(0).collider.gameObject.tag == "PlayerAttack")
        //{
            Destroy(gameObject, 0.25f);
        //}
    }
}
