using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToMouse : MonoBehaviour
{
    private Rigidbody2D rb;
    public float aimTolerance;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        
        float norm = Mathf.Atan2(1, 0) * Mathf.Rad2Deg;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, norm - aimTolerance, norm + aimTolerance);
        rb.SetRotation(Quaternion.AngleAxis(angle - 90, Vector3.forward));
        Debug.DrawRay(transform.position, dir, Color.green);
        Debug.DrawRay(transform.position, transform.rotation.eulerAngles, Color.red);
    }
}
