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
        else if(numOfDestinationsHit >0)
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

        CS1Timeline.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        CS1Timeline.gameObject.SetActive(false);
        Monster.GoToRandomDestination(targets);
    }
    IEnumerator RandomInteraction()
    {
        //do animation
        yield return new WaitForSeconds(1.0f);
        Monster.GoToRandomDestination(targets);
    }
   
       
    
    IEnumerator PlayCutscene()
    {
        //do cutscene

        yield return new WaitForSeconds(3.0f);
        cutscene1music.Stop();
        RiverGameMusic.Play();
        sceneController.proceedToRiver();
    }
}
