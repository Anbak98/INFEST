namespace Infest.Monster
{
    public struct MonsterStatus
    {
        public int original;
        public int offset;
        public int current;

        public MonsterStatus(int original, int offset)
        {
            this.original = original;
            this.offset = offset;
            this.current = original + offset;
        }
    }

    public interface IMonsterStatHandler
    {
        //   key": 1001,
        //  "Name": "PJ_H I",
        //  "MonsterType": 0,
        //  "MinHealth": 150,
        //  "MaxHealth": 250,
        //  "HealthPer5Min": 25,
        //  "MinAtk": 30,
        //  "MaxAtk": 42,
        //  "AtkPer5Min": 3,
        //  "MinDef": 20,
        //  "MaxDef": 28,
        //  "DefPer5Min": 2,
        //  "SpeedMove": 1.7,
        //  "SpeedMoveWave": 2.5,
        //  "SpeedAtk": 0.7,
        //  "DetectAreaNormal": 5,
        //  "DetectAreaWave": 100,
        //  "State": 200,
        //  "DropGold": 30,
        //  "FieldSpawn": true,
        //  "LimitSpawnCount": 9999
        MonsterInfo Info { get; }
        MonsterStatus Health { get; }
        MonsterStatus Damage { get; }
        MonsterStatus Defend { get; }
        void Init(int key, int playerCount, int elapsedTime);
        void ApplyDamage();
    }
}