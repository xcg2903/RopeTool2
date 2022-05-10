using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float forceDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Fire
        forceDirection = Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z;
        rb.AddForce(new Vector2(Mathf.Cos(forceDirection), Mathf.Sin(forceDirection)) * 1000);

        Destroy(gameObject, 10f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
