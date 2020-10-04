using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SoundTools;
public class bossManager : MonoBehaviour
{
    public int[] bosses;
    public static bossManager i;
    private void Start() {
        i = this;
        DontDestroyOnLoad(gameObject);
    }
    public void DestroyBoss(int boss){
        bosses[boss] = 1;
        StartCoroutine(LoadOverworld());
    }
    IEnumerator LoadOverworld(){
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Overworld");
    }
}