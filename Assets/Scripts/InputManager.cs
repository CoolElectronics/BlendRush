using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    public static InputManager e;
    public bool mouseClicked = false;
    public bool mouseHeld = false;
    public bool mouseHeldDown = false;
    public float mTimer = 0;
    public float HeldThreshold;
    void Start() {
        e = this;
    }
    void Update() {
        if (mouseHeldDown){
            mouseHeldDown = false;
        }
        if (mouseClicked){
            mouseClicked = false;
        }
        if (Input.GetMouseButtonDown(1)){
            mTimer = 0;
        }
        if (Input.GetMouseButton(1)){
            mTimer ++;
            if (mTimer == HeldThreshold){
                mouseHeld = true;
                mouseHeldDown = true;
            }
        }
        if (Input.GetMouseButtonUp(1)){
            if (!mouseHeld){
                mouseClicked = true;
            }
            mouseHeld = false;
            mTimer = 0;
        }      
    }
}