using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public InteractManager interact;
    public Player player;
    public Enemy enemy;
    public int tutorialStep;

    public GameObject step1;
    public GameObject step2;

    public bool nextStep;

    void Start()
    {
        interact = FindObjectOfType<InteractManager>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();

        tutorialStep = -1;
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Tutorial();
        }
    }

    public void Tutorial()
    {
        player.enabled = false;
        switch (tutorialStep)
        {
            case -1:
                if (!nextStep)
                {
                    nextStep = true;

                    if (player.floor == 60)
                        GameObject.Find("Flashlight").SetActive(false);
                    interact.fadeImg.color = new Color(0, 0, 0, 1);
                    StartCoroutine("FadeOut");
                }
                break;
            case 0:
                TutorialTalking(step1, 1.5f);
                break;
            case 1:
                TutorialTalking(step2, 0);
                break;
            case 2:
                player.enabled = true;
                player.onTutorial = false;
                enemy.gameObject.SetActive(true);
                SaveSystem.Save(player);
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

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2);
        for (float i = 1; i >= 0; i -= Time.deltaTime / 2)
        {
            interact.fadeImg.color = new Color(0, 0, 0, i);
            yield return null;
        }
        interact.fadeImg.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(2);

        tutorialStep++;
        nextStep = false;

        Tutorial();
    }
}
