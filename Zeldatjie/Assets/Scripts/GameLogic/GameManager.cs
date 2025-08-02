using System;
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

        private void Awake()
        {
            LoadTitle();
        }

        private void Update()
        {
            
            
        }

        public void LoadTitle()
        {
            // Load in the title scene
            SceneManager.LoadScene(_titleSceneName, LoadSceneMode.Additive);
            _currentGameState = GameState.Title;

        }
        
        public void EnterNextBattle()
        {
            SceneManager.UnloadSceneAsync(_titleSceneName);
            
            SceneManager.LoadScene(_battleScenes[_currentBattleIndex].SceneName, LoadSceneMode.Additive);
            _currentGameState = GameState.Fight;

        }
        
        
    }
}
