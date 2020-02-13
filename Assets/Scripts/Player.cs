using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject stuckTo;
    public float stickMultiplier = 100;
    private int layerMask = 1 << 10;
    public float castDist;
    private Rigidbody2D rigidbody;
    public float smoothingFactor;
    public float movementSpeed;
    private Vector2 stuckVelocity;
    public float aimLimit;
    private bool mouseState;
    private Vector2 prevMouseDir;
    private float pushedOff;
    public float pushOffWindow;

    // Start is called before the first frame update
    void Start()
    {
        stuckTo = null;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rigidbody.AddForce(movement);
       
    }

    public void StickTo(GameObject other)
    {
        stuckTo = other;
    }

    public void releaseFromStick()
    {
        stuckTo = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (Time.time - pushedOff > pushOffWindow && stuckTo == null)
        {
            if (collision.gameObject.layer == 10)
            {
                Rigidbody2D cR = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 v2 = ((rigidbody.velocity * rigidbody.mass) + (cR.velocity * cR.mass)) / (rigidbody.mass + cR.mass);
                cR.velocity = v2;
                stuckVelocity = v2;
                stuckTo = collision.gameObject;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (Time.time - pushedOff > pushOffWindow && collision.gameObject.layer == 10)
        {
            //ContactPoint2D hit; //= collision.GetContact(0);
            Vector2 norm = new Vector3();
            Vector2 hitP = new Vector3();

            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint2D hit = collision.GetContact(i);
                norm += hit.normal;
                hitP += hit.point;
            }
            norm /= collision.contactCount;
            hitP /= collision.contactCount;

            Vector3 normal = transform.TransformVector(norm);
            Vector3 hitPoint = transform.TransformPoint(hitP);

            Quaternion nr = Quaternion.FromToRotation(Vector3.up, norm);
            Vector3 rot = nr.eulerAngles;
            rot.y = 0;
            nr = Quaternion.Euler(rot);

            // Aiming Rotation
            if (Input.GetMouseButton(0))
            {
                mouseState = true;
                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 direction = new Vector3(
                    mousePosition.x - transform.position.x,
                    mousePosition.y - transform.position.y,
                    0
                );
                prevMouseDir = direction;
                Debug.DrawRay(transform.position, mousePosition, Color.magenta);

                Quaternion newRot = Quaternion.FromToRotation(Vector3.up, direction);
                Vector3 el = newRot.eulerAngles;
                
                //el.z = Mathf.Clamp(el.z + 360, rot.z - aimLimit, rot.z + aimLimit);
                el.y = 0;
                Debug.Log(el);
                gameObject.GetComponent<Rigidbody2D>().SetRotation(Quaternion.Euler(el));
            }
            else
            {
                if (mouseState)
                {
                    rigidbody.velocity = movementSpeed * prevMouseDir;
                    stuckTo = null;
                    mouseState = false;
                    pushedOff = Time.time;
                }
                else
                {
                    // Movement
                    float movedir = Input.GetAxis("Horizontal");
                    Vector3 movement = Vector3.Cross(norm, new Vector3(0, 0, 1.0f)).normalized * movedir * movementSpeed;
                    Debug.DrawRay(transform.position, movement, Color.cyan);

                    // Sticking
                    Vector3 stickDir = (transform.position - collision.gameObject.transform.position).normalized;
                    Debug.DrawRay(transform.position, stickDir, Color.yellow);
                    Vector3 stickForce = stickMultiplier * -norm.normalized;

                    Debug.DrawRay(hitP, norm, Color.green);
                    //Debug.Log("Hit Normal: " + normal);


                    // Opposite Object
                    Rigidbody2D cRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (cRigidbody.bodyType == RigidbodyType2D.Dynamic)
                    {
                        cRigidbody.velocity = (Vector2)(-1.0f * ((stickForce)) * (rigidbody.mass / cRigidbody.mass)) + stuckVelocity;
                    }

                    // Set Velocity of Player
                    rigidbody.velocity = (stuckVelocity + new Vector2(stickForce.x, stickForce.y) + new Vector2(movement.x, movement.y));
                    Debug.DrawRay(rigidbody.position, rigidbody.velocity, Color.red);
                }

                gameObject.GetComponent<Rigidbody2D>().SetRotation(nr);
            }
           
            
           
        }
    }
}
