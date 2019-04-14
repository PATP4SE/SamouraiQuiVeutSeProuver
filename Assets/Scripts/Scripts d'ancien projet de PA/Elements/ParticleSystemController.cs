using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<ParticleSystem>().isStopped)
		{
			GameObject.Destroy (gameObject.transform.parent.gameObject);
		}
	}
}
