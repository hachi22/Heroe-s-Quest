using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashFade : MonoBehaviour
{
    public Image splashImage;
    public Image splashImage2;
    public Image splashImage3;
    public Image splashImage4;
    public Image splashImage5;
    public string loadLevel;
    public static bool partidaNueva = false;

    IEnumerator Start()
    {
        partidaNueva = true;

        splashImage.canvasRenderer.SetAlpha(0.0f);
        splashImage2.canvasRenderer.SetAlpha(0.0f);
        splashImage3.canvasRenderer.SetAlpha(0.0f);
        splashImage4.canvasRenderer.SetAlpha(0.0f);
        splashImage5.canvasRenderer.SetAlpha(0.0f);

        yield return new WaitForSeconds(1.0f);
        FadeIn(splashImage);
        yield return new WaitForSeconds(10f);
        FadeOut(splashImage);
        yield return new WaitForSeconds(2.5f);

        yield return new WaitForSeconds(1.0f);
        FadeIn(splashImage2);
        yield return new WaitForSeconds(10f);
        FadeOut(splashImage2);
        yield return new WaitForSeconds(2.5f);

        yield return new WaitForSeconds(1.0f);
        FadeIn(splashImage3);
        yield return new WaitForSeconds(10f);
        FadeOut(splashImage3);
        yield return new WaitForSeconds(2.5f);

        yield return new WaitForSeconds(1.0f);
        FadeIn(splashImage4);
        yield return new WaitForSeconds(10f);
        FadeOut(splashImage4);
        yield return new WaitForSeconds(2.5f);

        yield return new WaitForSeconds(1.0f);
        FadeIn(splashImage5);
        yield return new WaitForSeconds(10f);
        FadeOut(splashImage5);
        yield return new WaitForSeconds(2.5f);

        
        SceneManager.LoadScene(loadLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(loadLevel);
        }
    }

    void FadeIn(Image image)
    {
        image.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut(Image image)
    {
        image.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}