using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] RiverGameManager riverManager;
    [SerializeField] StartingArea1 startAreaManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRiverSpelling()
    {
        SceneManager.LoadScene("MYDemo");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void proceedToRiver()
    {
        startAreaManager.gameObject.SetActive(false);
        riverManager.gameObject.SetActive(true);
    }
}
