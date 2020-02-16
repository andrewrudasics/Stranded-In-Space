using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTile: MonoBehaviour
{
	Renderer rend;

	public bool debug;
	[Range(0f, 1f)]
	public float scale;

    // Start is called before the first frame update
    void Start()
    {
		rend = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/*
	 * code from: https://answers.unity.com/questions/126206/auto-tile-texture.html
	 */
	void OnDrawGizmos()
	{
		if (debug == false) {
			rend.material.SetTextureScale("_MainTex", new Vector2(this.gameObject.transform.lossyScale.x * scale, this.gameObject.transform.lossyScale.y * scale));

		}
		else
		{
			this.gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(this.gameObject.transform.lossyScale.x * scale, this.gameObject.transform.lossyScale.y * scale));
		}

	}
}
