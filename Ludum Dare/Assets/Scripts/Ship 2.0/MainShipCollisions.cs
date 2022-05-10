using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShipCollisions : MonoBehaviour
{
    Thruster2 hitThruster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Thruster2>())
        {
            hitThruster = collision.gameObject.GetComponent<Thruster2>();
            hitThruster.AttachRocket(gameObject.GetComponentInParent<Rigidbody2D>().gameObject);
        }
    }
}
