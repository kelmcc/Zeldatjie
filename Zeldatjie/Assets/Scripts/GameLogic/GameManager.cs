using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeldatjie.Gameplay
{
    public class GameManager : MonoService
    {
        [SerializeField] private string _titleSceneName;
        [SerializeField] private List<SceneData> _battleScenes;
        [SerializeField] private Player _player;
        public GameState CurrentGameState => _currentGameState;
        private GameState _currentGameState;
        
        private int _currentBattleIndex = 0;
        private SceneRootLogic _currentSceneRootLogic = null;
        private Coroutine _loadingScene = null;

        public enum GameState
        {
            None,
            Title,
            Fight,
            Loot,
            Explore,
            Lose,
            Win
        }
        
        bool IsInGame => _currentGameState == GameState.Fight || _currentGameState == GameState.Loot || _currentGameState == GameState.Explore;

        private void Awake()
        {
            LoadTitle();
        }

        private void Update()
        {

            if (IsInGame)
            {
                _player.UpdatePlayer();
            }
        }

        private void LoadTitle()
        {
            SceneManager.LoadScene(_titleSceneName, LoadSceneMode.Additive);
            _currentGameState = GameState.Title;
        }
        
        public void EnterNextBattle()
        {
            if (_currentGameState == GameState.Title)
            {
                SceneManager.UnloadSceneAsync(_titleSceneName);
            }
            
            _currentGameState = GameState.None;
            
            if (_loadingScene != null)
            {
                StopCoroutine(_loadingScene);
            }
            _loadingScene = StartCoroutine(LoadAndFind(_battleScenes[_currentBattleIndex].SceneName, () =>
            {
                _currentGameState = GameState.Fight;
                _currentBattleIndex++;
                _currentSceneRootLogic.SetToFightMode();
            }));
        }
        
        
        private IEnumerator LoadAndFind(string targetSceneName, Action onComplete)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => op.isDone);

            Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
            if (targetScene.isLoaded)
            {
                GameObject[] rootObjects = targetScene.GetRootGameObjects();
                _currentSceneRootLogic = rootObjects[0].GetComponent<SceneRootLogic>();
                if (_currentSceneRootLogic == null)
                {
                    Debug.LogError("SceneRootLogic not found, make sure its on the first root object");
                }
                onComplete?.Invoke();
            }
        }
        
    }
}
