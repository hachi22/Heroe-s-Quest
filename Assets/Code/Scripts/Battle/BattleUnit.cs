using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;
   

    public Fighter Fighter { get; set; }

    public BattleHud Hud
    {
        get { return hud; }
    }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake() //Awake es para crear lo que tendrá x objeto antes de iniciar el juego 
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Fighter fighter)
    {
       Fighter = fighter;
        if (isPlayerUnit)
           image.sprite = Fighter.Base.LeftSprite;
        else
            image.sprite = Fighter.Base.RightSprite;
       
        image.color = originalColor;
        PlayEnterAnimation();

    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(500f,originalPos.y);

        }else image.transform.localPosition = new Vector3(-500f,originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f); //Para mover con una animacion, pasas la posicion y el tiempo (es del asset)

    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence(); //secuencia para hacer que realize la animación y luego vuelva a la posicion inicial
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.3f));

        }else sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.3f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.3f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        if (isPlayerUnit)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(image.DOColor(Color.red, 0f));
            sequence.Append(image.DOFade(0f, 1f));
        }
        else

        {
            var sequence = DOTween.Sequence();
            sequence.Append(image.DOColor(Color.red, 0f));
            sequence.Append(image.DOFade(0f, 1f));

        }
        
    }

}
