using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class pixelPerfectRendering : MonoBehaviour
{
    SpriteRenderer[] sprites;
    Vector3[] spritePos;
    void OnPreRender()
    {
        sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
        spritePos = new Vector3[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            Transform spriteT = sprites[i].gameObject.transform;
            spritePos[i] = spriteT.position;
            spriteT.position = new Vector3(spriteT.position.x, Mathf.Round(spriteT.position.y * 2) / 2, 0);
        }
    }

    void OnPostRender()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Transform spriteT = sprites[i].gameObject.transform;
            spriteT.position = spritePos[i];
        }
    }
}