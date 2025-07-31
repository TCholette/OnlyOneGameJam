using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float minY;

    [SerializeField] private GameObject flashLight;
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
        if (Input.GetKeyDown(KeyCode.F)) {
            flashLight.SetActive(!flashLight.activeInHierarchy);
        }
        Vector3 mousePos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0) - transform.position;
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        float mouseAngle = Mathf.Atan2(mousePos.y, mousePos.x) * 180 / Mathf.PI - 90;
        flashLight.transform.eulerAngles = new Vector3(0f,0f,mouseAngle);
    }
}
