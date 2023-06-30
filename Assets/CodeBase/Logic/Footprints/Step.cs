using UnityEngine;

namespace CodeBase.Logic.Footprints
{
    public class Step : MonoBehaviour
    {
        [SerializeField] private GameObject _footprintPrefab;

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.name == Constants.GroundLayer)
                SpawnDecal(_footprintPrefab);
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