using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip neutralZoneStart;
    [SerializeField]
    AudioClip neutralZoneCont;
    GameObject Player;
    public bool firingEngines = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        source = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (source.clip == neutralZoneStart || source.clip == neutralZoneCont)
        {
            firingEngines = Vector3.Distance(this.gameObject.transform.position, Player.transform.position) < 50f;
        }
    }
}
