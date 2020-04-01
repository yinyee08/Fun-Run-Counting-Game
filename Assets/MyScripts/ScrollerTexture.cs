using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerTexture : MonoBehaviour {
    public float scrollX = 0;
    public float scrollY = 0.5f;

    void Update(){

        float OffsetX = Time.timeScale * scrollX;
        float OffsetY = -Time.time / 15.0f;

        Debug.Log(Time.time);
        
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}