
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using UnityEngine.Analytics;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
public class AnalyticsManager : SingletonBehaviour<AnalyticsManager>
{
    async void Start()
    {
        try
        {
            // Unity Services 초기화
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log("Unity Services Initialized");

            /*//(1) 퍼널 이벤트 전송 예시
            SendFunnelStep("2");

            //(2) 스킬 이벤트 전송 예시
            analyticsSkill("1", "3");

            //(3) 플레이더 등급 전송 예시
            analyticsPlayerClass("5");

            //(4) 플레이어 레벨 정보 전송 예시
            analyticsPlayerUpgrade("2", "3");*/

            analyticsBeforeInGame(10, 0);

        }
        catch (System.Exception e)
        {
            Debug.LogError("Unity Services failed to initialize: " + e.Message);
        }
    }

    //=============================퍼널=================================
    //[1]퍼널
    public static void SendFunnelStep(string stepNumber)//das
    {
        var funnelEvent = new CustomEvent("Funnel_Step"); //event
        funnelEvent["Funnel_Step_Number"] = stepNumber; //parameter

        AnalyticsService.Instance.RecordEvent(funnelEvent); //custom event
    }

    //==========================커스텀 이벤트=============================

    public static void analyticsMatching(bool success, int matchingType = 0)
    {
        var customEvent = new CustomEvent("Matching_Info"); //event
        customEvent["SUCCESS_MATCH"] = success; //parameter
        if (matchingType != 0) customEvent["MATCHING_APPROACH_TYPE"] = matchingType; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsBeforeInGame(int mode, int playerClass)
    {
        var customEvent = new CustomEvent("Before_InGame");
        customEvent["MODE_NUMBER"] = mode; //parameter
        customEvent["PLAYER_CLASS"] = playerClass; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event\

        
    }

    public static void analyticsZombieKill(int zombie)
    {
        var customEvent = new CustomEvent("Zombie_Kill"); //event
        customEvent["ZOMBIE_DIE_COUNT"] = zombie; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsPlayerDie(int count, int time, int wave, string transfrom)
    {
        var customEvent = new CustomEvent("Player_Die"); //event
        customEvent["PLAYER_DIE_COUNT"] = count; //parameter
        customEvent["DIE_TIME"] = time; //parameter
        customEvent["DIE_WAVE"] = wave; //parameter
        customEvent["DIE_TRANSFROM"] = transfrom; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }
    public static void analyticsWave(int wavereason, int waveTime, int zombieSpawn, int firstTime = 0)
    {
        var customEvent = new CustomEvent("Wave_Event"); //event
        customEvent["WAVE_EVENT"] = wavereason; //parameter
        customEvent["FIRST_WAVE_TIME"] = firstTime; //parameter
        customEvent["WAVE_TIME"] = waveTime; //parameter
        customEvent["WAVE_ZOMBIE_SPAWN"] = zombieSpawn; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }
    public static void analyticsEnterBoss(int zombie, int enterTime, int enterCount)
    {
        var customEvent = new CustomEvent("Enter_Boss_Battle"); //event
        customEvent["ZOMBIE_KILL_BEFORE_ENTER"] = zombie; //parameter
        customEvent["BOSS_BATTLE_ENTER_TIME"] = enterTime; //parameter
        customEvent["BOSS_BATTLE_ENTER_COUNT"] = enterCount; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsGameEnd(int endReason, int ingameToLobby, int ingameTime, int lastWeapon)
    {
        var customEvent = new CustomEvent("Game_End"); //event
        customEvent["END_REASON"] = endReason; //parameter
        customEvent["INGAME_TO_LOBBY"] = ingameToLobby; //parameter
        customEvent["INGAME_TIME"] = ingameTime; //parameter
        customEvent["LAST_WEAPON"] = lastWeapon; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsUseItem(int itemType, int itemIndex, int whenUse)
    {
        var customEvent = new CustomEvent("Use_Item"); //event
        customEvent["ITEM_TYPE_USE"] = itemType; //parameter
        customEvent["USE_ITEM_INDEX"] = itemIndex; //parameter
        customEvent["WHEN_USE"] = whenUse; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsShopPopupOpen(int count, int playerCoin)
    {
        var customEvent = new CustomEvent("Shop_Popup_Open"); //event
        customEvent["INTERACTION_COUNT"] = count; //parameter
        customEvent["PLAYER_COIN"] = playerCoin; //parameter

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsPurchase(int weapon)
    {
        var customEvent = new CustomEvent("Purchase_Item"); //event
        customEvent["WEAPON_NUMBER"] = weapon; //parameter
        

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

    public static void analyticsSell(int weapon)
    {
        var customEvent = new CustomEvent("Sell_Item"); //event
        customEvent["WEAPON_SELL_NUMBER"] = weapon; //parameter
        

        AnalyticsService.Instance.RecordEvent(customEvent); //custom event
    }

}
#endif