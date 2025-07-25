using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float time = 0.05f;
	
	private float timer;
	public bool Attacking = true;
	
	void Start ()
	{
		timer = time;
		StartCoroutine("Flicker");
	}
	
	IEnumerator Flicker()
	{
		while(true)
		{
			// Set light enabled if Attacking and not previously enabled
			GetComponent<Light>().enabled = Attacking && (!GetComponent<Light>().enabled);
			
			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			timer = time;
		}
	}
}
