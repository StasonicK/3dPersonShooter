using UnityEngine;

namespace CodeBase.Logic.Footprints
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private GameObject _footprintPrefab;
        [SerializeField] private float _footprintSpacer = 1.0f;

        private Vector3 _lastFootprint;
        private bool _isTouchingGround;

        private void Start() =>
            _lastFootprint = transform.position;

        private void Update()
        {
            // if (_isTouchingGround)
            // {
            //     float distanceSinceLastFootprint = Vector3.Distance(_lastFootprint, transform.position);
            //
            //     if (distanceSinceLastFootprint >= _footprintSpacer)
            //     {
            //         SpawnDecal(_footprintPrefab);
            //         _lastFootprint = transform.position;
            //     }
            // }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == Constants.GroundLayer)
            {
                _isTouchingGround = true;
                SpawnDecal(_footprintPrefab);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.name == Constants.GroundLayer)
                _isTouchingGround = false;
        }

        private void SpawnDecal(GameObject prefab)
        {
            var transform1 = transform;
            var position = transform1.position;

            Vector3 from = position;
            Vector3 to = new Vector3(position.x,
                position.y - (transform1.localScale.y / 2.0f) + 0.1f, position.z);
            Vector3 direction = to - from;

            RaycastHit hit;

            if (Physics.Raycast(from, direction, out hit))
            {
                GameObject decal = Instantiate(prefab);
                decal.transform.position = hit.point;
                decal.transform.Rotate(Vector3.up, transform.eulerAngles.y);
            }
        }
    }
}