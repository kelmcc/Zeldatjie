using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeldatjie.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Scene _titleScene;
        [SerializeField] private List<SceneData> _battleScenes;
        [SerializeField] private Player _player;
        public GameState CurrentGameState => _currentGameState;
        private GameState _currentGameState;

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

        private void Update()
        {
            
            
        }
    }
}
