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
        public StoreController storeController;
        public InputManager inputManager;

        public event Action OnChangeGameState;

        private void OnEnable()
        {
            OnChangeGameState += ChangeStore;
        }

        private void OnDisable()
        {
            OnChangeGameState -= ChangeStore;
        }

        public void ChangeState(GameState state)
        {
            gameState = state;
            OnChangeGameState?.Invoke(); 
        }

        public void ChangeStore()
        {
            if (gameState == GameState.None)
            {
                storeController.Activate();
            }
            else
            {
                storeController.Deactivate();
            }
        }

        protected override void Awake()
        {
            base.Awake();
#if UNITY_WEBGL
            Application.targetFrameRate = 60;
#endif
        }

        public void VictoryGame()
        {
            FindAnyObjectByType<InputManager>().SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Global.Instance.UIManager.Hide<UIStateView>();
            Global.Instance.UIManager.Hide<UIDeathScreen>();
            GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();

            int weaponOneKey = 0;

            if(gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[0] != null)
            {
                weaponOneKey = gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[0].instance.data.Key;
            }
            int weaponTwoKey = 0;

            if (gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[1] != null)
            {
                weaponTwoKey = gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[1].instance.data.Key;
            }

            AnalyticsManager.analyticsGameEnd(1, (int)Runner.SimulationTime, weaponOneKey, weaponTwoKey);
            gev.Victory();
        }

        public void DefeatGame()
        {
            RPC_BroadcastDefeatGame();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_BroadcastDefeatGame()
        {
            FindAnyObjectByType<InputManager>().SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Global.Instance.UIManager.Hide<UIStateView>();
            Global.Instance.UIManager.Hide<UIDeathScreen>();
            GameEndView gev = Global.Instance.UIManager.Show<GameEndView>();
            int weaponOneKey = 0;

            if (gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[0] != null)
            {
                weaponOneKey = gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[0].instance.data.Key;
            }
            int weaponTwoKey = 0;

            if (gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[1] != null)
            {
                weaponTwoKey = gamePlayers.GetPlayerObj(Runner.LocalPlayer).Weapons._weapons[1].instance.data.Key;
            }

            AnalyticsManager.analyticsGameEnd(2, (int)Runner.SimulationTime, weaponOneKey, weaponTwoKey);
            gev.Defeat();
        }
    }
}