using CodeBase.Logic.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float _damage;

    [HideInInspector] public Vector3 Direction;

    private void Start() =>
        Destroy(gameObject, timeToDestroy);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}