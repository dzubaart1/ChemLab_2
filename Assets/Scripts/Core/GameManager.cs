using System;
using System.Collections;
using JetBrains.Annotations;
using LocalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public const string LOBBY_SCENE_NAME = "LobbyScene";
        public const string FINISH_SCENE_NAME = "FinishScene";
        public const string START_SCENE_NAME = "StartScene";
        public const string LAB_1_SCENE_NAME = "Lab1";
        public const string LAB_2_SCENE_NAME = "Lab2";
        public const string LAB_3_SCENE_NAME = "Lab2";
        
        public event Action<string> LoadSceneCompleteEvent;
        
        [CanBeNull] public static GameManager Instance { get; private set; }
        [CanBeNull] public BaseLocalManager CurrentBaseLocalManager { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void SetLocalManger(BaseLocalManager localManager)
        {
            CurrentBaseLocalManager = localManager;
        }
        
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName);
            
            while (!loadingScene.isDone)
            {
                yield return null;
            }
            
            LoadSceneCompleteEvent?.Invoke(sceneName);
        }
    }
}