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

	private MagneticPole myPole;
	private CapsuleCollider2D capsule;
	private Transform trans;
	private LineRenderer lr;
	private SpriteRenderer sr;
	private Color blue = new Color(0.388f, 0.595f, 0.915f, 1f);
	private Color red = new Color(0.981f, 0.389f, 0.389f, 1f);

	// Start is called before the first frame update
	void Start()
	{
		capsule = this.GetComponent<CapsuleCollider2D>();
		trans = this.transform;
		lr = this.GetComponent<LineRenderer>();
		sr = this.transform.parent.GetComponent<SpriteRenderer>();
		if (pole == MagneticPole.N)
		{
			sr.color = red;
			myPole = MagneticPole.N;
		}
		else
		{
			sr.color = blue;
			myPole = MagneticPole.S;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (pole != myPole)
		{
			if (pole == MagneticPole.N)
			{
				sr.color = red;
				myPole = MagneticPole.N;
			}
			else
			{
				sr.color = blue;
				myPole = MagneticPole.S;
			}
		}
		DrawCapsule();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Magnet magnetComp = collision.GetComponent<Magnet>();
		if (magnetComp != null)
		{
			Vector3 magnetForce = CalculateGilbertForce(this, magnetComp);
			// this.transform.root.GetComponent<Rigidbody2D>().AddForce(magnetForce);
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

	/*
	 * Draws the magnetic field
	 */
	void DrawCapsule()
	{
		float rotation = trans.rotation.eulerAngles.z;
		Vector2 colliderSize = trans.lossyScale * capsule.size;

		// if one is smaller, emulate it to a circle collider
		if (capsule.direction == CapsuleDirection2D.Horizontal && colliderSize.x < colliderSize.y)
		{
			colliderSize.x = colliderSize.y;
		}
		if (capsule.direction == CapsuleDirection2D.Vertical && colliderSize.x > colliderSize.y)
		{
			colliderSize.y = colliderSize.x;
		}
		Vector2 colliderCenter = trans.TransformPoint((Vector2)trans.localPosition + capsule.offset);

		if (capsule.direction == CapsuleDirection2D.Horizontal)
		{
			float radius = colliderSize.y / 2;
			float lengthToCurve = (colliderSize.x - colliderSize.y) / 2;
			Vector2 topRight = RotateAroundPivot(new Vector2(colliderCenter.x + lengthToCurve,
				colliderCenter.y + radius),
				colliderCenter,
				rotation);
			Vector2 topLeft = RotateAroundPivot(new Vector2(colliderCenter.x - lengthToCurve,
				colliderCenter.y + radius),
				colliderCenter,
				rotation);
			Vector2 botRight = RotateAroundPivot(new Vector2(colliderCenter.x + lengthToCurve,
				colliderCenter.y - radius),
				colliderCenter,
				rotation);
			Vector2 botLeft = RotateAroundPivot(new Vector2(colliderCenter.x - lengthToCurve,
				colliderCenter.y - radius),
				colliderCenter,
				rotation);
			Vector3 rightCurveCenter = RotateAroundPivot(new Vector3(colliderCenter.x + lengthToCurve,
				colliderCenter.y, 0),
				colliderCenter,
				rotation);
			Vector3 leftCurveCenter = RotateAroundPivot(new Vector3(colliderCenter.x - lengthToCurve,
				colliderCenter.y, 0),
				colliderCenter,
				rotation);
			int segments = 180;
			int pointCounts = 2 * segments + 5;
			Vector3[] points = new Vector3[pointCounts];
			points[0] = topLeft;
			points[1] = topRight;

			for (int i = 0; i < segments; i++)
			{
				float rad = Mathf.Deg2Rad * (i * 180f / segments);
				points[i + 2] = RotateAroundPivot(new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0) + rightCurveCenter,
					rightCurveCenter,
					rotation);
			}

			points[segments + 2] = botRight;
			points[segments + 3] = botLeft;

			for (int i = 0; i < segments; i++)
			{
				float rad = Mathf.Deg2Rad * (180f + i * 180f / segments);
				points[i + segments + 4] = RotateAroundPivot(new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0) + leftCurveCenter,
					leftCurveCenter,
					rotation);
			}

			points[2 * segments + 4] = topLeft;

			lr.positionCount = pointCounts;
			lr.SetPositions(points);
		}
		else if (capsule.direction == CapsuleDirection2D.Vertical)
		{
			float radius = colliderSize.x / 2;
			float lengthToCurve = (colliderSize.y - colliderSize.x) / 2;
			Vector2 topRight = RotateAroundPivot(new Vector2(colliderCenter.x + radius,
				colliderCenter.y + lengthToCurve),
				colliderCenter,
				rotation);
			Vector2 topLeft = RotateAroundPivot(new Vector2(colliderCenter.x - radius,
				colliderCenter.y + lengthToCurve),
				colliderCenter,
				rotation);
			Vector2 botRight = RotateAroundPivot(new Vector2(colliderCenter.x + radius,
				colliderCenter.y - lengthToCurve),
				colliderCenter,
				rotation);
			Vector2 botLeft = RotateAroundPivot(new Vector2(colliderCenter.x - radius,
				colliderCenter.y - lengthToCurve),
				colliderCenter,
				rotation);
			Vector3 topCurveCenter = RotateAroundPivot(new Vector3(colliderCenter.x,
				colliderCenter.y + lengthToCurve, 0),
				colliderCenter,
				rotation);
			Vector3 botCurveCenter = RotateAroundPivot(new Vector3(colliderCenter.x,
				colliderCenter.y - lengthToCurve, 0),
				colliderCenter,
				rotation);
			int segments = 180;
			int pointCounts = 2 * segments + 5;
			Vector3[] points = new Vector3[pointCounts];
			points[0] = botLeft;
			points[1] = topLeft;

			for (int i = 0; i < segments; i++)
			{
				float rad = Mathf.Deg2Rad * (270f + i * 180f / segments);
				points[i + 2] = RotateAroundPivot(new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0) + topCurveCenter,
					topCurveCenter,
					rotation);
			}

			points[segments + 2] = topRight;
			points[segments + 3] = botRight;

			for (int i = 0; i < segments; i++)
			{
				float rad = Mathf.Deg2Rad * (90f + i * 180f / segments);
				points[i + segments + 4] = RotateAroundPivot(new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0) + botCurveCenter,
					botCurveCenter,
					rotation);
			}

			points[2 * segments + 4] = botLeft;

			lr.positionCount = pointCounts;
			lr.SetPositions(points);
		}
	}

	public static Vector2 RotateAroundPivot(Vector2 aPoint, Vector2 aPivot, float aDegree)
	{
		aPoint -= aPivot;
		float rad = aDegree * Mathf.Deg2Rad;
		float s = Mathf.Sin(rad);
		float c = Mathf.Cos(rad);
		return aPivot + new Vector2(
			aPoint.x * c - aPoint.y * s,
			aPoint.y * c + aPoint.x * s
		);
	}
}
