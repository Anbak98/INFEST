using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;


public class AnalyticsManager : SingletonBehaviour<AnalyticsManager>
{
    //매칭 이벤트
    public static void AnalyticsMatchingInfo(bool successMatch, 
        int matchingCodeCopy, int matchingApproachType)
    {
        var matchingEvent = new CustomEvent("Matching_Info"); //event
        matchingEvent["SUCCESS_MATCH"] = successMatch;
        matchingEvent["MATCHING_CODE_COPY"] = matchingCodeCopy;
        matchingEvent["MATCHING_APPROACH_TYPE"] = matchingApproachType;

        AnalyticsService.Instance.RecordEvent(matchingEvent);
    }

    //인게임 이벤트
    public static void AnalyticsGameStart(int modeNumber = 0, int playerClass = 0, int playerDieCount = 0,
        int waveEvent = 0, int zombieDieCount = 0, bool bossZombieDie = false, int ItemTypeUse = 0)
    {
        var gameEvent = new CustomEvent("Matching_Info"); //event
        if (modeNumber != 0)
            gameEvent["MODE_NUMBER"] = modeNumber;
        
        if (playerClass != 0)
            gameEvent["PLAYER_CLASS"] = playerClass;
        if (playerDieCount != 0)
            gameEvent["PLAYER_DIE_COUNT"] = playerDieCount;
        if (waveEvent != 0)
            gameEvent["WAVE_EVENT"] = waveEvent;
        if (zombieDieCount != 0)
            gameEvent["ZOMBIE_DIE_COUNT"] = zombieDieCount;
        if (bossZombieDie != false)
            gameEvent["BOSS_ZOMBIE_DIE"] = bossZombieDie;
        if (ItemTypeUse != 0)
            gameEvent["ITEM_TYPE_USE"] = ItemTypeUse;

        AnalyticsService.Instance.RecordEvent(gameEvent);
    }

    //상점 이벤트
    public static void AnalyticsShopInfo(int interactionCount = 0, int timeOfUse = 0,
        int weaponNumber = 0, int weaponSellNumber = 0, int consumeItemNumber = 0)
    {
        var shopEvent = new CustomEvent("Shop_Info");

        if (interactionCount != 0)
            shopEvent["INTERACTION_COUNT"] = interactionCount;
        if (timeOfUse != 0)
            shopEvent["TIME_OF_USE"] = timeOfUse;
        if (weaponNumber != 0)
            shopEvent["WEAPON_NUMBER"] = weaponNumber;
        if (weaponSellNumber != 0)
            shopEvent["WEAPON_SELL_NIUMBER"] = weaponSellNumber;
        if (consumeItemNumber != 0)
            shopEvent["CONSUMEITEM_NUMBER"] = consumeItemNumber;

        AnalyticsService.Instance.RecordEvent(shopEvent);
    }

}
