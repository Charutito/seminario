using BattleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EnemyEntity : Entity
    {
        [SerializeField] private Gradient _LifeGradient;
        [SerializeField] private Color _Color;
        [SerializeField] private Renderer _Renderer;


        protected override void OnUpdate()
        {
            _Color = _LifeGradient.Evaluate(Stats.Health.Current / Stats.Health.Max);
            _Renderer.material.color = _Color;
        }

        private void Start()
        {
            Stats.Health.OnActualChange += (float old, float current) =>
            {
                if (current == Stats.Health.Min)
                {
                    Destroy(gameObject);
                }
            };
        }
    }
}
