﻿using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/pix")]
public class pix : MonoBehaviour
{
    public int h = 64;
    int w;
    protected void Start()
    {
    }
    void Update()
    {
 
        float ratio = ((float)Camera.main.pixelWidth) / (float)Camera.main.pixelHeight;
        w = Mathf.RoundToInt(h * ratio);
 
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}