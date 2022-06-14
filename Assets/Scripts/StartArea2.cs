using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea2 : MonoBehaviour
{

    [SerializeField] private List<AudioSource> Cutscene2VoiceLines = new List<AudioSource>();
    [SerializeField] private List<Transform> targets = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCutscene2());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayCutscene2()
    {
        //Add camera movements 

        Cutscene2VoiceLines[0].Play();
        yield return new WaitForSeconds(4.0f);

        Cutscene2VoiceLines[1].Play();
        yield return new WaitForSeconds(6.0f);

        Cutscene2VoiceLines[2].Play();
        yield return new WaitForSeconds(4.0f);

        Cutscene2VoiceLines[3].Play();
        yield return new WaitForSeconds(5.0f);

        Cutscene2VoiceLines[4].Play();
        yield return new WaitForSeconds(6.0f);

        Cutscene2VoiceLines[5].Play();
        yield return new WaitForSeconds(12.0f);
    }
}
