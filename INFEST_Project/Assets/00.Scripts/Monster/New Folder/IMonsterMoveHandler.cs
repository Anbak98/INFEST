using UnityEngine;

namespace Infest.Monster
{
    public interface IMonsterMoveHandler
    {
        void SetTarget(Transform target);
        void Move();
        bool MoveRandomly(float minDistance, float maxDistance, float radius);
    }
}