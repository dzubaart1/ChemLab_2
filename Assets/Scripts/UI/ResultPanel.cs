using Core;
using UnityEngine;
using Core.Services;
using JetBrains.Annotations;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [Header("UIs")]
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _errorsText;
    
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            return;
        }

        if (gameManager.CurrentBaseLocalManager == null)
        {
            return;
        }
        
        _timeText.text = "Время прохождения: " + gameManager.CurrentBaseLocalManager.GetGameTime();
        _errorsText.text = "Количество ошибок: " + gameManager.CurrentBaseLocalManager.GetErrorTasks().Count;
    }
}
