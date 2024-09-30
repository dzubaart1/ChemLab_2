using System;
using System.Collections;
using System.Globalization;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

namespace UI.Components
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldComponent : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public string Value;
        }
        
        [SerializeField] private InputFieldType _inputFieldType;

        private TasksService _tasksService;
        private SaveService _saveService;
        private UIService _uiService;

        private TMP_InputField _inputField;
        
        private SavedData _savedData;

        private bool _isReadyToEnter = true;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onSelect.AddListener(OnSelect);
            _inputField.shouldHideSoftKeyboard = true;
            _inputField.shouldHideMobileInput = true;

            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _uiService = Engine.GetService<UIService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _inputField.onSelect.RemoveListener(OnSelect);
        }

        private void OnSelect(string value)
        {
            
            _uiService.Keyboard.transform.SetParent(transform);
            
            Vector3 newKeyBoardLocalPos = _uiService.Keyboard.transform.localPosition;
            
            _uiService.Keyboard.transform.localPosition = newKeyBoardLocalPos;

            RectTransform rectTransform = _uiService.Keyboard.GetComponent<RectTransform>();
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchoredPosition3D = new Vector3 (0, -2, 2);
            rectTransform.localRotation = Quaternion.identity;

            _uiService.Keyboard.InputField = _inputField;
            _uiService.Keyboard.OnTextSubmitted += OnTextSubmitted;
            _uiService.Keyboard.PresentKeyboard();
        }

        private void OnTextSubmitted(object sender, EventArgs e)
        {
            if (!_isReadyToEnter)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_inputField.text))
            {
                return;
            }

            try
            {
                var fValue = float.Parse(_inputField.text, CultureInfo.InvariantCulture.NumberFormat);
                _tasksService.TryCompleteTask(new InputFieldActivity(_inputFieldType, fValue));
                _uiService.Keyboard.OnTextSubmitted -= OnTextSubmitted;
                _uiService.Keyboard.Close();
                StartCoroutine(DelayBetweenEnter());
            }
            catch (Exception)
            {
                Debug.Log("Can't convert");
            }
        }

        public void OnSaveScene()
        {
            _savedData.Value = _inputField.text;
        }

        public void OnLoadScene()
        {
            _inputField.text = _savedData.Value;
            _uiService.Keyboard.Close();
        }

        private IEnumerator DelayBetweenEnter()
        {
            _isReadyToEnter = false;
            yield return new WaitForSeconds(0.2f);
            _isReadyToEnter = true;
        }
    }
}