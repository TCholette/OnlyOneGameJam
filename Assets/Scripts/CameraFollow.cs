using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float minY;

    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY) {
            cam.transform.position = new Vector3(transform.position.x, minY, cam.transform.position.z);
        } else {
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        }
    }
}
