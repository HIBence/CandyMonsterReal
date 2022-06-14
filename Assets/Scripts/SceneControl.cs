using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    [SerializeField] RiverGameManager riverManager;
    [SerializeField] StartingArea1 startAreaManager;
    [SerializeField] StartArea2 startArea2;
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
    public void Quit()
    {
        Application.Quit();
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

    public void proceedToStartArea2()
    {
        riverManager.gameObject.SetActive(false);
        startArea2.gameObject.SetActive(true);
    }
}
