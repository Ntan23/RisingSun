using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    #region ForPopUp
    public void HoverFX() 
    {
        if(!gm.GetIsShowingPopUp()) LeanTween.scale(gameObject, new Vector2(1.1f, 1.1f), 0.25f);
    }

    public void UnhoverFX() 
    {
        if(!gm.GetIsShowingPopUp()) LeanTween.scale(gameObject,Vector2.one, 0.25f);
    }
    #endregion

    #region Other
    public void ButtonHoverFX() => LeanTween.scale(gameObject, new Vector2(1.1f, 1.1f), 0.25f);

    public void ButtonUnhoverFX() => LeanTween.scale(gameObject,Vector2.one, 0.25f);
    #endregion
}
