using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gm;
    private GameObject stuckTo, prevStuck;
    public float stickMultiplier = 100;
    private int layerMask = 1 << 10;
    public float castDist;
    private Rigidbody2D rigidbody;
    public float smoothingFactor;
    public float movementSpeed, pushOffSpeed;
    private Vector2 stuckVelocity;
    public float aimTolerance;
    private bool mouseState, grounded;
    private Vector2 prevMouseDir;
    private float pushedOff;
    public float pushOffWindow;
    private Vector3 cNorm;

    // Start is called before the first frame update
    void Start()
    {
        stuckTo = null;
        grounded = true;
        gm = GameManager.Instance;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Aiming Rotation
        if (Input.GetMouseButtonUp(0)) {
            GameManager.Logger.LogLevelAction(400 + (gm.GetLevelBuildIndex() - 1), "Player jumped in Level " + (gm.GetLevelBuildIndex() - 1));

        }
        if (Input.GetMouseButton(0) && grounded)
        {
            mouseState = true;
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            prevMouseDir = dir;



            /* Angle approach 
            float angle = Vector3.SignedAngle(cNorm, dir, Vector3.forward);
            //Debug.Log("Angle: " + angle);
            angle = Mathf.Clamp(angle, -aimTolerance, aimTolerance);
            Debug.Log("Angle: " + angle);
            float nrmAngle = Vector3.Angle(Vector3.up, cNorm);

            
            Debug.Log("nrmAngle: " + nrmAngle);
            */

            // Quaternion Approach

            // Local Angle Approach
            Vector3 dirLocal = transform.InverseTransformDirection(dir);
            Vector3 normLocal = transform.InverseTransformDirection(cNorm);
            float angle = Vector3.SignedAngle(normLocal, dirLocal, new Vector3(0, 0, 1));
            
            rigidbody.rotation = Vector3.SignedAngle(Vector3.up, cNorm, Vector3.forward) + angle;


            //float nrmLocal = Mathf.Atan2(normLocal.y, normLocal.x) * Mathf.Rad2Deg;
            //float angleLocal = Mathf.Atan2(dirLocal.y, dirLocal.x) * Mathf.Rad2Deg;

            //Debug.Log(angleLocal);

            //float nrm = Mathf.Atan2(cNorm.y, cNorm.x) * Mathf.Rad2Deg;
            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //float currAngle = Mathf.Atan2(transform.up.y, transform.up.x);

            //angleLocal = Mathf.Clamp(angleLocal, -aimTolerance, aimTolerance);
            //rigidbody.SetRotation(Quaternion.AngleAxis(nrmAngle + angle, Vector3.forward));



            //Debug.Log(rigidbody.rotation);
            Debug.DrawRay(transform.position, dir, Color.green);
            Debug.DrawRay(transform.position, transform.rotation.eulerAngles, Color.red);

/*
            Quaternion up = Quaternion.Euler(Vector3.up);
            Quaternion aim = Quaternion.Euler(direction.normalized);
            Quaternion sNorm = Quaternion.Euler(cNorm);
            float angle = Quaternion.Angle(up, aim);
            Debug.Log("Aim Angle:" + angle);
            
            float normAngle = Quaternion.Angle(up, sNorm);
            Debug.Log("Norm Angle:" + normAngle);
            angle = Mathf.Clamp(angle, normAngle - aimLimit, normAngle + aimLimit);
            aim = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

            Quaternion newRot = Quaternion.FromToRotation(Vector3.up, direction);
            Vector3 el = newRot.eulerAngles;
            Debug.Log(el);
            el.y = 0;
            gameObject.GetComponent<Rigidbody2D>().SetRotation(Quaternion.Euler(el));
            */
        } 
        else if(!Input.GetMouseButton(0))
        {


            if (mouseState)
            {
                rigidbody.velocity = pushOffSpeed * prevMouseDir.normalized * (prevMouseDir.magnitude * 0.4f);
                stuckTo.GetComponent<Rigidbody2D>().velocity = -prevMouseDir / stuckTo.GetComponent<Rigidbody2D>().mass * pushOffSpeed;
                prevStuck = stuckTo;
                stuckTo = null;
                mouseState = false;
                pushedOff = Time.time;
                grounded = false;
            }
        }
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
		SetMagnetStick(true);
		if ((Time.time - pushedOff > pushOffWindow) && stuckTo == null)
        {
            if (collision.gameObject.layer == 10)
            {
				grounded = true;
                Rigidbody2D cR = collision.gameObject.GetComponent<Rigidbody2D>();
                stuckTo = collision.gameObject;
                if (cR.bodyType == RigidbodyType2D.Dynamic)
                {
                    Vector2 v2 = ((rigidbody.velocity * rigidbody.mass) + (cR.velocity * cR.mass)) / (rigidbody.mass + cR.mass);
                    cR.velocity = v2;
                    stuckVelocity = v2;
                }
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

            cNorm = norm;

            Vector3 normal = transform.TransformVector(norm);
            Vector3 hitPoint = transform.TransformPoint(hitP);

            Quaternion nr = Quaternion.FromToRotation(Vector3.up, norm);
            Vector3 rot = nr.eulerAngles;
            rot.y = 0;
            nr = Quaternion.Euler(rot);

            Vector3 movement;
            
            if (!Input.GetMouseButton(0))
            {
                // Movement
                float movedir = Input.GetAxis("Horizontal");
                movement = Vector3.Cross(norm, new Vector3(0, 0, 1.0f)).normalized * movedir * movementSpeed;
                Debug.DrawRay(transform.position, movement, Color.cyan);
            }
            else
            {
                movement = new Vector3();
            }
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

            if (!Input.GetMouseButton(0))
            {
                gameObject.GetComponent<Rigidbody2D>().SetRotation(nr);
            }
            
           
            
           
        }
    }

	private void OnCollisionExit2D(Collision2D collision)
	{
		SetMagnetStick(false);
	}

	// tries to disable magnet if magnet found
	private void SetMagnetStick(bool isStickTo)
	{
		Magnet mag = this.GetComponentInChildren<Magnet>();
		// if not same pole dont set stick
		if (mag != null && !mag.isSamePole())
		{
			this.GetComponentInChildren<Magnet>().SetStickTo(isStickTo);
		}
	}
}
