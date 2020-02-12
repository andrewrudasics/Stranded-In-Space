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

    // Start is called before the first frame update
    void Start()
    {
        stuckTo = null;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, castDist, layerMask);
        Debug.DrawRay(transform.position, castDist * -transform.up, Color.red);
        if (hit.collider != null)
        {
            Vector3 normal = transform.TransformVector(hit.normal);
            Vector3 hitPoint = transform.TransformPoint(hit.point);
            gameObject.GetComponent<Rigidbody2D>().SetRotation(Quaternion.FromToRotation(transform.up, transform.InverseTransformDirection(normal)));

            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.Log("Hit Normal: " + normal);
        }
        */
        //gameObject.GetComponent<Rigidbody2D>().AddForce(stickMultiplier * (stuckTo.transform.position - transform.position).normalized);
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

    private void OnCollisionStay2D(Collision2D collision)
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
        //gameObject.GetComponent<Rigidbody2D>().
        //gameObject.GetComponent<Rigidbody2D>().SetRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, transform.InverseTransformDirection(normal)), smoothingFactor /** Time.deltaTime*/));
        gameObject.GetComponent<Rigidbody2D>().SetRotation(Quaternion.FromToRotation(transform.up, normal));

        float movedir = Input.GetAxis("Horizontal");
        Vector3 movement = Vector3.Cross(norm, new Vector3(0,0,1.0f)).normalized * movedir * movementSpeed;
        Debug.DrawRay(transform.position, movement, Color.cyan);
        Vector3 stickDir = (transform.position - collision.gameObject.transform.position).normalized;
        Debug.DrawRay(transform.position, stickDir, Color.yellow);
        Vector3 stickForce = stickMultiplier * -norm.normalized; //+ stickDir;
        //rigidbody.AddForce(movement + stickForce);
        Debug.DrawRay(hitP, norm, Color.green);
        Debug.Log("Hit Normal: " + norm);
        rigidbody.velocity = ((movement + stickForce));
        Debug.DrawRay(rigidbody.position, rigidbody.velocity, Color.red);
        
    }
}
