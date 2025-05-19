using Fusion;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace INFEST.Game
{
    public enum GameState
    {
        None = 1,
        Wave = 2,
        Boss = 3
    }

    public class NetworkGameManager : SingletonNetworkBehaviour<NetworkGameManager>
    {
        [Networked] public TickTimer GameTimer { get; set; }
        [Networked] public GameState gameState { get; set; } = GameState.None;

        public EnhancedMonsterSpawner monsterSpawner;
        public GamePlayerHandler gamePlayers;

        public void VictoryGame()
        {
            GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();
            //AnalyticsManager.analyticsGameEnd(1, 0, Runner.SimulationTime, )
            gev.Victory();
        }
        public void DefeatGame()
        {
            GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();
            //AnalyticsManager.analyticsGameEnd(2, )
            gev.Defeat();
        }
    }
}