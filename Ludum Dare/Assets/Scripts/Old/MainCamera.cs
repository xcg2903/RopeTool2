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

    // Start is called before the first frame update
    void Start()
    {
        currTarget = GameObject.FindGameObjectWithTag("Player");
        mainCamera = gameObject.GetComponent<Camera>();
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
    }

    // can be called to change the camera zoom
    public void SetScale(int newScale)
    {
        // mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newScale, scaleLerpDuration);
        // mainCamera.orthographicSize += newScale / scaleLerpDuration;
    }
}
