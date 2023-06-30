using UnityEngine;

namespace CodeBase.Logic.Footprints
{
    public class Footprint : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 2.0f;

        private float _mark;
        private Vector3 _size;

        public void Start()
        {
            _mark = Time.time;
            _size = transform.localScale;
        }

        public void Update()
        {
            float elapsedTime = Time.time - _mark;

            if (elapsedTime != 0)
            {
                float percentTimeLeft = (_lifetime - elapsedTime) / _lifetime;

                transform.localScale = new Vector3(_size.x * percentTimeLeft, _size.y * percentTimeLeft,
                    _size.z * percentTimeLeft);

                if (elapsedTime > _lifetime)
                    Destroy(gameObject);
            }
        }
    }
}