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

    private List<Transform> targets = new List<Transform>();
    private int numOfDestinationsHit = 0;


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
        else if(numOfDestinationsHit < 4)
        {
            StartCoroutine(RandomInteraction());
        }
        else if (numOfDestinationsHit == 4)
        {
            StartCoroutine(InteractThenGoToCutscene());
            
        }
        else
        {
            StartCoroutine(PlayCutscene());
            

        }
        numOfDestinationsHit++;
        
    }

    IEnumerator IntroInteraction()
    {
        //do animation
        yield return new WaitForSeconds(23.0f);
        Monster.GoToRandomDestination(targets);
    }
    IEnumerator RandomInteraction()
    {
        //do animation
        yield return new WaitForSeconds(3.0f);
        Monster.GoToRandomDestination(targets);
    }
    IEnumerator InteractThenGoToCutscene()
    {
        //do animation
        yield return new WaitForSeconds(3.0f);
        Monster.SetDestinationTransform(cutscene1Position.transform);
    }
    IEnumerator PlayCutscene()
    {
        //do cutscene

        yield return new WaitForSeconds(5.0f);
        cutscene1music.Stop();
        RiverGameMusic.Play();
        sceneController.proceedToRiver();
    }
}
