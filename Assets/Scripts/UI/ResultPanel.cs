using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Services;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _timeText;
    [SerializeField]private TextMeshProUGUI _errorsText;

    private TasksService _tasksService;

    private void Awake()
    {
        _tasksService = Engine.GetService<TasksService>();

        _timeText.text = "Время прохождения: ";
    }
}
