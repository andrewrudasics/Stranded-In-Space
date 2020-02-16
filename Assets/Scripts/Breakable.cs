using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
	[Range(0f, 20f)]
	public float collisionSlowDown;
	[Range(0f, 1f)]
	public float breakTime;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			this.GetComponent<Collider2D>().isTrigger = false;
		}
		else
		{
			Vector2 vel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
			collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-vel * collisionSlowDown);
			Destroy(gameObject, breakTime);
		}
	}
}
