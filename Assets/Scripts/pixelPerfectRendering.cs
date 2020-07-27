using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class pixelPerfectRendering : MonoBehaviour
{
    SpriteRenderer[] sprites;
    Vector3[] spritePos;
    private void Start()
    {

    }
    void OnPreRender()
    {
        if (this.enabled)
        {
            sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
            spritePos = new Vector3[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                Transform spriteT = sprites[i].gameObject.transform;
                if (spriteT.parent == null || !spriteT.parent.gameObject.GetComponent<SpriteRenderer>())
                {
                    spritePos[i] = spriteT.position;
                    spriteT.position = new Vector3(spriteT.position.x, Mathf.Round(spriteT.position.y * 2) / 2, 0);
                }
                else
                {
                    spritePos[i] = new Vector3(9.99f,9.99f,9.99f);
                }
            }
        }
    }

    void OnPostRender()
    {
        if (this.enabled)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (spritePos[i] != new Vector3(9.99f,9.99f,9.99f)){
                    Transform spriteT = sprites[i].gameObject.transform;
                    spriteT.position = spritePos[i];
                }
            }
        }
    }
}