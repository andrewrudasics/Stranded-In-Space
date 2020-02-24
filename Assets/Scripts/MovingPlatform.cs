using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	private Vector3 posA;
	private Vector3 posB;
	private Vector3 nextPos;

	[SerializeField]
	private Transform child;

	[SerializeField]
	private float speed;

	[SerializeField]
	private Transform tranB;
    // Start is called before the first frame update
    void Start()
    {
    	posB = tranB.localPosition;
    	posA = child.localPosition;
     	nextPos = posB;   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() 
    {
    	child.localPosition = Vector3.MoveTowards(child.localPosition, nextPos, speed * Time.deltaTime);

    	if (Vector3.Distance(child.localPosition, nextPos) <= 0.1) 
    	{
    		ChangeDest();
    	}
    }

    private void ChangeDest() {
    	nextPos = nextPos != posA ? posA : posB;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    	if (collision.gameObject.tag.Equals("Player"))
    	{
    		collision.collider.transform.SetParent(transform);
    	}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    	if (collision.gameObject.tag.Equals("Player"))
    	{
    		collision.collider.transform.SetParent(null);
    	}
    }
}
