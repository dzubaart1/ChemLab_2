using BioEngineerLab.Core;
using BioEngineerLab.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalPanel : BasePanel<DesctopPanelsType>
{
    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private List<Sprite> _endCardGIFsprites;
    [SerializeField] private Image _endCardImage;
    [SerializeField] private TextMeshProUGUI _timeCount;
    [SerializeField] private TextMeshProUGUI _mistakesCount;
    [SerializeField] private TextMeshProUGUI _mistakesText;
    private TasksService _tasksService;

    private void Awake()
    {
        _tasksService = Engine.GetService<TasksService>();
    }

    public void OnEndTask()
    {
        gameObject.SetActive(true);
        PlayEndCard();
        var date = _tasksService.GetCurrentGameTime();
        _timeCount.text = date.Hours + ":" + date.Minutes + ":" + date.Seconds;
        _mistakesCount.text = _tasksService.GetErrorsList().Count.ToString();

        if (_tasksService.GetErrorsList().Count == 0)
        {
            _mistakesText.text = "";
            return;
        }
        _mistakesText.text = "Вы ошиблись в следующих заданиях:\n";
        foreach (var error in _tasksService.GetErrorsList())
        {
            _mistakesText.text += error.TaskNumber + ". " + error.TaskText + "\n";
        }
    }

    public void PlayEndCard()
    {
        StartCoroutine(PlaySpriteAnimation());
    }

    private IEnumerator PlaySpriteAnimation()
    {
        while (true)
        {
            foreach (var sprite in _endCardGIFsprites)
            {
                _endCardImage.sprite = sprite;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void SwitchToResultsPanel()
    {
        _resultsPanel.SetActive(true);
    }
    public void SwitchToMainPanel()
    {
        _resultsPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
