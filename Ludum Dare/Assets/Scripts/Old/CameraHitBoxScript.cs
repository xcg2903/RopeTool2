using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHitBoxScript : MonoBehaviour
{
    [SerializeField]
    float newCameraScale;
    GameObject mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject);
            mainCamera.GetComponent<MainCamera>().targetScale = newCameraScale;
        }
    }
}
