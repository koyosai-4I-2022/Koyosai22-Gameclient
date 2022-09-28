using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCameraController : MonoBehaviour
{
    Quaternion cameraRot, chRot;

    bool cursorLock = true;

    float minX = -90f, maxX = 90f;

    void Start()
    {
        cameraRot = this.transform.localRotation;
        chRot = this.transform.localRotation;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X") * 2f;
        float my = Input.GetAxis("Mouse Y") * 2f;

        cameraRot *= Quaternion.Euler(-my, 0, 0);
        chRot *= Quaternion.Euler(0, mx, 0);

        cameraRot = ClampRotation(cameraRot);

        this.transform.localRotation = cameraRot;
        this.transform.localRotation = chRot;

        UpdateCursorLock();
    }
	private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        this.transform.position += new Vector3(x, 0, y) * Time.deltaTime * 8f;
    }
	public void UpdateCursorLock()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if(Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if(cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public Quaternion ClampRotation(Quaternion q)
    {

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
