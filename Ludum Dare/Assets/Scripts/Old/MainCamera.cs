using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    GameObject currTarget;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    int currScale;
    [SerializeField]
    float scaleLerpDuration;
    public float targetScale;
    public float scaleBounds = .2f;

    //STRETCH
    public float height = 1.0f;
    public float width = 1.0f;
    Rigidbody2D rb;
    const float stretchSpeed = 30.0f;
    const float camAcc = 1.0f;
    float cameraSize;

    // Start is called before the first frame update
    void Start()
    {
        currTarget = GameObject.FindGameObjectWithTag("Player");
        mainCamera = gameObject.GetComponent<Camera>();
        rb = currTarget.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(currTarget.transform.position.x, currTarget.transform.position.y, -10);
        if (mainCamera.orthographicSize <= targetScale - scaleBounds || mainCamera.orthographicSize >= targetScale + scaleBounds)
        {
            if (mainCamera.orthographicSize < targetScale)
            {
                mainCamera.orthographicSize += scaleLerpDuration * Time.deltaTime;
            } else
            {
                mainCamera.orthographicSize -= scaleLerpDuration * Time.deltaTime;
            }
        }

        //STRETCH CODE
        cameraSize = 0;
        if(Mathf.Abs(rb.velocity.x) > stretchSpeed)
        {
            width = Mathf.Lerp(width, Mathf.Abs(rb.velocity.x) / stretchSpeed, Time.deltaTime * camAcc);
        }
        else
        {

            width = Mathf.Lerp(width, 1.0f, Time.deltaTime * (camAcc / 2));
        }
        if (Mathf.Abs(rb.velocity.y) > stretchSpeed)
        {
            height = Mathf.Lerp(height, Mathf.Abs(rb.velocity.y) / stretchSpeed, Time.deltaTime * camAcc);
        }
        else
        {
            height = Mathf.Lerp(height, 1.0f, Time.deltaTime * (camAcc / 2));
        }


        //stretch view//
        mainCamera.ResetProjectionMatrix();
        mainCamera.orthographicSize = 13 + width + height;
        Matrix4x4 m = mainCamera.projectionMatrix;

        m.m11 *= height;
        m.m00 *= width;
        mainCamera.projectionMatrix = m;
    }

    // can be called to change the camera zoom
    public void SetScale(int newScale)
    {
        // mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newScale, scaleLerpDuration);
        // mainCamera.orthographicSize += newScale / scaleLerpDuration;
    }
}
