using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EagleMovement : MonoBehaviour 
{ 
    public float speed = 5.5f;
    private int knockdownOnce = 0;
    private bool knockdown = false;

    void Start(){
        Debug.Log("Here");
    }

    void Update(){
        if(!knockdown)
            this.transform.Translate(0,0,speed*Time.deltaTime);
    }

    void OnTriggerEnter()
	{
		Debug.Log("GetHit");
		knockdown = true;
        this.GetComponent<Animator>().SetTrigger("Hit");
        GameManager.playHit = true;
        knockdown = false;
        knockdownOnce++;
        if(knockdownOnce == 1)
            GameManager.livesCount --;
	}
}