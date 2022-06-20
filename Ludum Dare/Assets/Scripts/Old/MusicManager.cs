using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip neutralZoneStart;
    [SerializeField]
    AudioClip neutralZoneCont;

    private float timeElapsed = 0;

    private bool hasTriggered = false;
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if(!hasTriggered && hit.gameObject.tag == "Player")
        {
            source.Stop();
            source.clip = neutralZoneStart;
            hasTriggered = true;
            source.loop = false;
            source.volume = .9f;
            source.Play();
        }
    }

    public void Update()
    {
        if(source.clip == neutralZoneStart)
        {
            timeElapsed += Time.deltaTime;
        }
        if(timeElapsed >= neutralZoneStart.length)
        {
            source.Stop();
            source.loop = true;
            source.clip = neutralZoneCont;
            source.Play();
            timeElapsed = 0;
        }
    }
}
