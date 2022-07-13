using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
