using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject stuckTo;
    public float stickMultiplier = 100;


    // Start is called before the first frame update
    void Start()
    {
        stuckTo = null;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(stickMultiplier * (stuckTo.transform.position - transform.position).normalized);
    }

    public void StickTo(GameObject other)
    {
        stuckTo = other;
    }

    public void releaseFromStick()
    {
        stuckTo = null;
    }
}
