using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
	[System.Serializable]
	public enum MagneticPole { N, S };

	[Range(10.0f, 700.0f)]
	public float magnetForce = 4f;
	public MagneticPole pole = MagneticPole.N;
	public float permeability = 0.05f;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Magnet magnetComp = collision.GetComponent<Magnet>();
		if (magnetComp != null)
		{
			Vector3 magnetForce = CalculateGilbertForce(this, magnetComp);
			this.transform.parent.GetComponent<Rigidbody2D>().AddForce(magnetForce);
		}
	}

	/*
	 * Since we only have monopole magnets the formula
	 * F = (permeability * magnet1 force * magnet2 force) / 4 * pi * r^2
	 * can be used
	 * code from https://steemit.com/programming/@kubiak/magnet-simulation-in-unity
	 */
	Vector3 CalculateGilbertForce(Magnet magnet1, Magnet magnet2)
	{
		Vector3 m1 = magnet1.transform.position;
		Vector3 m2 = magnet2.transform.position;
		Vector3 r = m2 - m1;
		float dist = r.sqrMagnitude;
		float part0 = permeability * magnet1.magnetForce * magnet2.magnetForce;
		float part1 = 4 * Mathf.PI * dist;

		float f = (part0 / part1);

		if (magnet1.pole == magnet2.pole)
		{
			f = -f;
		}

		return f * r.normalized;
	}
}
