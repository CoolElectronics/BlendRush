using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject gameUIPanel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActivateGameUI()
    {
        gameUIPanel.SetActive(true);
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject go in objects)
        {
            Behaviour[] scripts = go.GetComponents<Behaviour>();
            foreach (Behaviour c in scripts)
            {
                c.enabled = false;
            }
        }
    }
    public void DeactivateGameUI()
    {
        gameUIPanel.SetActive(false);
    }
}
