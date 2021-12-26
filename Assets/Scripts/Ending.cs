using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public TextMeshProUGUI prologue;
    public TextMeshProUGUI complete;

    public TextMeshProUGUI makers;
    public TextMeshProUGUI names;

    public TextMeshProUGUI press;

    public bool isEnd;

    void Start()
    {
        StartCoroutine("End");   
    }

    private void Update()
    {
        if (isEnd)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime));
            yield return null;
        }
    }
    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator End()
    {
        StartCoroutine("FadeInText", prologue);
        StartCoroutine("FadeInText", complete);

        yield return new WaitForSeconds(3);

        StartCoroutine("FadeOutText", prologue);
        StartCoroutine("FadeOutText", complete);

        yield return new WaitForSeconds(2);

        StartCoroutine("FadeInText", makers);
        StartCoroutine("FadeInText", names);

        yield return new WaitForSeconds(3);

        StartCoroutine("FadeOutText", makers);
        StartCoroutine("FadeOutText", names);

        yield return new WaitForSeconds(2);

        StartCoroutine("FadeInText", press);
        isEnd = true;
    }
}
