using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingArea1 : MonoBehaviour
{
    [SerializeField] GameObject targetsHolder;
    [SerializeField] GameObject introPosition;
    [SerializeField] GameObject cutscene1Position;
    [SerializeField] AudioSource cutscene1music;
    [SerializeField] AudioSource startArea1VOiceLine;
    [SerializeField] AudioSource RiverGameMusic;
    [SerializeField] MonsterMovement Monster;
    [SerializeField] SceneControl sceneController;
    [SerializeField] GameObject CS1Timeline;
    [SerializeField] private List<AudioSource> Cutscene1VoiceLines = new List<AudioSource>();

    private List<Transform> targets = new List<Transform>();
    private int numOfDestinationsHit = 0;
    private bool proceeding = false;


    // Start is called before the first frame update
    void Start()
    {
        
        
        cutscene1music.Play();
        int i =0;
        while(i < targetsHolder.gameObject.transform.childCount ){
            targets.Add(targetsHolder.transform.GetChild(i));
            i++;
        }
        Monster.SetDestinationTransform(introPosition.transform);
        Monster.reachedDestination.AddListener(MonsterReachedDestination);

    }

    // Update is called once per frame
    void Update()
    {
        Monster.UpdateStartingArea1Methods();
    }

    private void MonsterReachedDestination()
    {
        if(numOfDestinationsHit == 0)
        {
            startArea1VOiceLine.Play();
            //start animation
            StartCoroutine(IntroInteraction());
        }
        else if (proceeding)
        {
            StartCoroutine(PlayCutscene());
        }
        else 
        {
            StartCoroutine(RandomInteraction());
        }
        
        numOfDestinationsHit++;
        
    }
    public void ProceedFromArea1()
    {
        if (!proceeding)
        {
            Monster.SetDestinationTransform(cutscene1Position.transform);
            proceeding = true;

        }
        
    }

    IEnumerator IntroInteraction()
    {
        //do animation

        yield return new WaitForSeconds(23.0f);
        Cutscene1VoiceLines[3].Play();
        if (!proceeding) {
            Monster.GoToRandomDestination(targets);
        }

       
        
    }
    IEnumerator RandomInteraction()
    {
        //do animation
        yield return new WaitForSeconds(1.0f);
        if (!proceeding) Monster.GoToRandomDestination(targets);
    }
   
       
    
    IEnumerator PlayCutscene()
    {
        //enable timeline for camera movements

        Cutscene1VoiceLines[0].Play();
        //do river camera movement
        yield return new WaitForSeconds(4.0f);

        Cutscene1VoiceLines[1].Play();
        //move camera to dinokill
        yield return new WaitForSeconds(8.0f);
        
        Cutscene1VoiceLines[2].Play();
        //move camera back to reegie
        yield return new WaitForSeconds(8.0f);

        Cutscene1VoiceLines[4].Play();
        //move camera back to reegie
        yield return new WaitForSeconds(6.0f);





        cutscene1music.Stop();
        RiverGameMusic.Play();
        sceneController.proceedToRiver();
    }
}
