using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
    public Vector3 axis = new Vector3(1f, 1f, 1f);
    [Range(-360f, 360f)]
    public float angle = 0f;
    [Range(0f, 10f)]
    public float speed = 2f;

    [SerializeField]
    GameObject rod;

    Vector3 toDefault;

    void Start()
    {
        toDefault = rod.transform.GetChild(0).position;
    }

    void Update()
    {
        this.transform.position = new Vector3(
            Mathf.Cos(Time.time * speed),
            Mathf.Sin(Time.time * speed) * Mathf.Cos(Time.time * speed),
            Mathf.Sin(Time.time * speed) ) * 10f;

        var RodToCube = this.transform.position - rod.transform.position;

        //Debug.DrawLine(Vector3.zero, toDefault * 4f, Color.blue);
        //Debug.DrawLine(Vector3.zero, RodToCube, Color.red);
        Debug.DrawLine(Vector3.zero, rod.transform.TransformPoint(Vector3.up), Color.red);

        float angle_x = 0f;
        float angle_y = Mathf.Atan2(RodToCube.z, RodToCube.x) * Mathf.Rad2Deg;
        float angle_z = Mathf.Atan2(RodToCube.y, new Vector2(RodToCube.x, RodToCube.z).magnitude) * Mathf.Rad2Deg;

        Debug.Log($"{angle_x}:{angle_y}:{angle_z}");

        rod.transform.rotation = Quaternion.Euler(angle_x, -angle_y, angle_z);

        //rod.transform.LookAt(this.transform, Vector3.right);
        //this.transform.localRotation = Quaternion.AngleAxis(angle, axis);

        //Debug.DrawLine(Vector3.zero, axis, Color.red);
    }
}
