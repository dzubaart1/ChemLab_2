using Core;
using UnityEngine;
using Core.Services;
using JetBrains.Annotations;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _errorsText;

    [CanBeNull] private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        if (_gameManager == null)
        {
            return;
        }
        
        _timeText.text = "Время прохождения: " + (_gameManager.Game.GameFinishTime - _gameManager.Game.GameStartTime);
        _errorsText.text = "Количество ошибок: " + _gameManager.Game.Errors.Count;
    }
}
