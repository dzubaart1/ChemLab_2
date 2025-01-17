using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("Panels")] 
        [SerializeField] private RectTransform _startPanel;
        [SerializeField] private RectTransform _lab1Panel;
        [SerializeField] private RectTransform _lab2Panel;
        [SerializeField] private RectTransform _lab3Panel;
        [SerializeField] private RectTransform _resultPanel;

        [Space]
        [Header("UIs")]
        [SerializeField] private Button _resultLoadButton;
        [SerializeField] private Button _lab1LoadButton;
        [SerializeField] private Button _lab2LoadButton;
        [SerializeField] private Button _lab3LoadButton;
        
        private RectTransform _currentRectTransform;

        private void OnEnable()
        {
            _lab1LoadButton.onClick.AddListener(Lab1Open);
            _lab2LoadButton.onClick.AddListener(Lab2Open);
            _lab3LoadButton.onClick.AddListener(Lab3Open);
            _resultLoadButton.onClick.AddListener(ResultOpen);
        }

        private void OnDisable()
        {
            _lab1LoadButton.onClick.RemoveListener(Lab1Open);
            _lab2LoadButton.onClick.RemoveListener(Lab2Open);
            _lab3LoadButton.onClick.RemoveListener(Lab3Open);
            _resultLoadButton.onClick.RemoveListener(ResultOpen);
        }

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            
            _resultLoadButton.gameObject.SetActive(gameManager != null && gameManager.IsGameFinished);
            
            _startPanel.gameObject.SetActive(false);
            _lab1Panel.gameObject.SetActive(false);
            _lab2Panel.gameObject.SetActive(false);
            _lab3Panel.gameObject.SetActive(false);
            _resultPanel.gameObject.SetActive(false);
            
            _currentRectTransform = _startPanel;
            _currentRectTransform.gameObject.SetActive(true);
        }
    
        public void LoadLab1()
        {
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.OnSelectLab(ELab.Lab1);
        }
        
        public void LoadLab2()
        {
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.OnSelectLab(ELab.Lab2);
        }
        
        public void LoadLab3()
        {
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.OnSelectLab(ELab.Lab3);
        }

        public void StartOpen()
        {
            SwitchPanel(_startPanel);
        }

        private void ResultOpen()
        {
            SwitchPanel(_resultPanel);
        }
    
        private void Lab1Open()
        {
            SwitchPanel(_lab1Panel);
        }
        
        private void Lab2Open()
        {
            SwitchPanel(_lab2Panel);
        }
        
        private void Lab3Open()
        {
            SwitchPanel(_lab3Panel);
        }
        
        private void SwitchPanel(RectTransform panel)
        {
            _currentRectTransform.gameObject.SetActive(false);
            _currentRectTransform = panel;
            _currentRectTransform.gameObject.SetActive(true);
        }
    }
}