using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables
    [SerializeField] Enemy[] enemies;

    //Properties
    public Enemy[] Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
