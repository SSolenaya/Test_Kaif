using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(Plane))]
    public class PlayGround : MonoBehaviour
    {
        [SerializeField] private Plane _plane;
        [SerializeField] private float _spawnRange = 5f;

        public Vector3 GetRandomPoint()
        {
            Vector3 spawnCenter = Vector3.zero;
            Vector3 randomPointOnPlane = _plane.ClosestPointOnPlane(spawnCenter + _spawnRange * Random.insideUnitSphere);
            randomPointOnPlane.x = randomPointOnPlane.x * transform.localScale.x;
            randomPointOnPlane.z = randomPointOnPlane.z * transform.localScale.z;
            Vector3 resultVec = new Vector3(randomPointOnPlane.x, 5, randomPointOnPlane.z);
            return resultVec;
        }
    }
}
