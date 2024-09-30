using System;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.UI;

public class KrussButtonCompjnent : ButtonComponent
{
    [HideInInspector]
    public bool IsOn;

    [SerializeField] private Image _btnImage;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private GameObject _lamp;

    protected override void OnClickBtn()
    {
        IsOn = !IsOn;
        _lamp.SetActive(IsOn);
        UpdateSprite();

        base.OnClickBtn();
    }

    private void UpdateSprite()
    {
        _btnImage.sprite = IsOn ? _onSprite : _offSprite;
    }

    public void OnLoadScene(bool state)
    {
        IsOn = state;
        UpdateSprite();
    }
}
