using Fusion;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace INFEST.Game
{
    public enum GameState
    {
        None,
        Wave,
        Boss
    }

    public class NetworkGameManager : SingletonNetworkBehaviour<NetworkGameManager>
    {
        [Networked] public TickTimer GameTimer { get; set; }
        [Networked] public GameState gameState { get; set; } = GameState.None;

        public EnhancedMonsterSpawner monsterSpawner;
        public GamePlayerHandler gamePlayers;

        public int playerCount = 0;

        public void VictoryGame()
        {
            //GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();
            //gev.Victory();
        }
        public void DefeatGame()
        {
            //GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();
            //gev.Defeat();
        }
    }
}