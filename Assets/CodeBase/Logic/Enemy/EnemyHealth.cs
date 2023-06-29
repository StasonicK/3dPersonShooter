using System;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    public class EnemyHealth : MonoBehaviour,IHealth
    {
        public float Health;
        public bool IsDead { private set; get; }

        private void Start()
        {
            IsDead = false;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0f)
                Die();
        }

        private void Die()
        {
            IsDead = true;
        }
    }
}