using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public InteractManager interact;
    public Player player;
    public int tutorialStep;

    public GameObject step1;
    public GameObject step2;

    public bool nextStep;

    void Start()
    {
        interact = FindObjectOfType<InteractManager>();
        player = FindObjectOfType<Player>();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Tutorial();
    }

    public void Tutorial()
    {
        player.enabled = false;
        switch (tutorialStep)
        {
            case 0:
                TutorialTalking(step1, 1);
                break;
            case 1:
                TutorialTalking(step2, 0);
                break;
            case 2:
                player.enabled = true;
                player.onTutorial = false;
                break;
        }
    }

    public void TutorialTalking(GameObject step ,float delayTime)
    {
        if (!nextStep)
            interact.Talking(step);

        if (interact.page == 0 && !nextStep)
        {
            nextStep = true;
            StartCoroutine("Delayer", delayTime);
        }
    }

    IEnumerator Delayer(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        tutorialStep++;
        nextStep = false;

        Tutorial();
    }
}
