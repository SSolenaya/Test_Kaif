using UnityEngine;

namespace Assets.Scripts
{
    public class PlayGroundController : MonoBehaviour
    {
        public static PlayGroundController Inst;
        [SerializeField] private Transform _playGround;
        [SerializeField] private Transform _worldParent;
        [SerializeField] private float _spawnRange = 5f;
        private float _offset => MainController.Inst.globalParams.boardMapOffset;

        private void Awake()
        {
            if (Inst == null)
            {
                Inst = this;
            }
        }

        public Vector3 GetRandomPoint()
        {
            float x = Random.Range(-_playGround.localScale.x + _offset, _playGround.localScale.x - _offset) * 0.5f + _playGround.position.x;
            float z = Random.Range(-_playGround.localScale.z + _offset, _playGround.localScale.z - _offset) * 0.5f + _playGround.position.z;
            Vector3 resultVec = new Vector3(x, 0, z);
            return resultVec;
        }

        public bool IsThisPointOnPlayground(Vector3 point)
        {
            float minX = -_playGround.localScale.x * 0.5f + _offset;
            float minZ = -_playGround.localScale.z * 0.5f + _offset;
            float maxX = _playGround.localScale.x * 0.5f - _offset;
            float maxZ = _playGround.localScale.z * 0.5f - _offset;
            return point.x <= maxX && point.x > minX && point.z <= maxZ && point.z > minZ;
        }
    }
}