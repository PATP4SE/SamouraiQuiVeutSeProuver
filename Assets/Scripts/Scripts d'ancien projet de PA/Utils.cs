using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils  {


	public static void DecelerateX(ref Rigidbody2D rbody, float decelerationPercentage)
	{
		if (rbody != null)
		{
			if (!(rbody.velocity.x <= 0.0001 && rbody.velocity.x >= -0.0001 ))
			{
				float percent = (100f - decelerationPercentage);
				rbody.velocity = new Vector3(rbody.velocity.x * (percent / 100f), rbody.velocity.y);
			}
		}
	}
}
