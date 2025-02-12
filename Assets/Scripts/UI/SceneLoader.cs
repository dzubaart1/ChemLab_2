using UnityEngine;
using Core;
using UI.Components;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _button;
        [SerializeField] private ELab _lab;
        [SerializeField] LobbyUI _lobby;

        private void OnEnable()
        {
            _button.ClickBtnEvent += OnButtonClicked;
        }

        private void OnDisable()
        {
            _button.ClickBtnEvent -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            switch (_lab)
            {
                case ELab.Lab1: 
                    _lobby.LoadLab1();
                    break;
                case ELab.Lab2:
                    _lobby.LoadLab2();
                    break;
                case ELab.Lab3:
                    _lobby.LoadLab3();
                    break;
            }
        }
    } 
}
