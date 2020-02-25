using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool grounded, aiming, prevAiming;
    private Vector3 prevMouseDir;
    private Rigidbody2D rb;
    private


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
        {
            aiming = Input.GetMouseButton(0);

            // Rotation
            if (aiming)
            {
                // Get Mouse Direction
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 dir = Input.mousePosition - pos;
                prevMouseDir = dir;

                //Delete this later
                Vector3 cNorm = new Vector3();
                //Delete this later

                // Calculate Local Angle Variance from Normal
                Vector3 dirLocal = transform.InverseTransformDirection(dir);
                Vector3 normLocal = transform.InverseTransformDirection(cNorm);
                float angle = Vector3.SignedAngle(normLocal, dirLocal, new Vector3(0, 0, 1));

                // Set new rotation
                rb.rotation = Vector3.SignedAngle(Vector3.up, cNorm, Vector3.forward) + angle;
            }
            else
            {
                if (prevAiming != aiming)
                {
                    // Push Off
                }
                else
                {
                    // Set rotation
                    // Calculate CCW-CW Movement
                    // 
                }
            }

            prevAiming = aiming;

            

            // Player Movement

            // Set Rotation
        }
    }

    private void OnCollisionEnter2D()
    {
        grounded = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }
}
