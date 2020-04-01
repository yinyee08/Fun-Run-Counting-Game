using UnityEngine;
using System.Collections;

public class HitObject : MonoBehaviour 
{

	void OnTriggerEnter()
	{
		this.GetComponent<MeshRenderer>().enabled = false;
		GameManager.hit_count_object ++;
	}
}
