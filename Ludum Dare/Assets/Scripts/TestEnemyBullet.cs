using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * 20.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.GetContact(0).collider.gameObject.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
    }
}
