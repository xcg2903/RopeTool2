using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] Flyer[] flyers;

    //Properties
    public Flyer[] Flyers
    {
        get { return flyers; }
        set { flyers = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        flyers = FindObjectsOfType<Flyer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
