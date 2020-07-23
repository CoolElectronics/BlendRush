 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameLimit : MonoBehaviour
{
    public int fCount;
 void Awake () {
     QualitySettings.vSyncCount = 0;  // VSync must be disabled
     Application.targetFrameRate = fCount;
 }
}