using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowDeathScreen : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text subtitle;
    public TMP_Text score;
    public List<Button> buttons = new List<Button>();

    void Start()
    {
        title.gameObject.SetActive(false);
        subtitle.gameObject.SetActive(false);
        title.alpha = 0;
        subtitle.alpha = 0;
        score.alpha = 0;
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].gameObject.SetActive(false);

    }
    public void show()
    {
        buttons[1].gameObject.SetActive(true);
        score.text = System.Math.Round(ScoreManager.Instance.currentHealth, 2).ToString().ToString();
        title.gameObject.SetActive(true);
        subtitle.gameObject.SetActive(true);
        StartCoroutine(FadeTo(1, 0.8f));
    }

    public void hide()
    {
        title.gameObject.SetActive(false);
        subtitle.gameObject.SetActive(false);

        for (int i = 0; i < buttons.Count; i++)
            buttons[i].gameObject.SetActive(false);

        title.alpha = 0;
        subtitle.alpha = 0;
        score.alpha = 0;
    }

    IEnumerator FadeTo(float a, float time)
    {
        yield return new WaitForSeconds(1f);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            float alpha = Mathf.Lerp(title.alpha, a, t);
            title.alpha = alpha;
            yield return null;
        }
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time * 2)
        {
            float alpha = Mathf.Lerp(subtitle.alpha, a, t);
            subtitle.alpha = alpha;
            score.alpha = alpha;
            yield return null;
        }

        for (int i = 0; i < buttons.Count; i++)
            buttons[i].gameObject.SetActive(true);
    }
}
