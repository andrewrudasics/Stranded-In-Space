using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Sprite s1;
    Sprite s2;
    private bool dragging;
    private float moveSpeed = (float)2.0; 
    private Vector2 targetPos;
    public GameObject activePrefab;

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        dragging = false;

        targetPos = transform.position;
        //rb = gameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetKeyDown(KeyCode.Mouse0)) 
         {
             if (Input.GetMouseButtonDown(0)) 
             {
        		dragging = true;
                //spriteRenderer.sprite = s2;
        	}
        }

        if(Input.GetMouseButtonUp(0))
        {
            dragging = false;
            move();
            //rb.velocity = 2.0f * Vector3.up;
     
            //spriteRenderer.sprite = s1; //sets sprite renderers sprite
        }

        if (dragging)
        {
            faceMouse();
        }
    }

    void faceMouse()
    {
    	Vector3 mousePosition = Input.mousePosition;
    	mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    	Vector2 direction = new Vector2(
    		mousePosition.x - transform.position.x,
    		mousePosition.y - transform.position.y
    	);

    	transform.up = direction;
    }

    void move()
    {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //targetPos.z = transform.position.z;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed);
    }
    
}
