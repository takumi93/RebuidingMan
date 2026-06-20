using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitReticle : MonoBehaviour
{
    [SerializeField]
    float fadeSpeed = 0.1f;

    Image reticle = null;
    float alpha = 0f;

    void Start()
    {
        reticle = GetComponent<Image>();
    }

    private void Update()
    {
        reticle.color = new Color(1,1,1,alpha);
        if (alpha >= 0)
        {
            StartCoroutine(FadeOut());
        }
    }
    public void Show()
    {
        alpha = 1;
    }

    IEnumerator FadeOut()
    {
        alpha -= fadeSpeed;
        yield return null;
    }
}
