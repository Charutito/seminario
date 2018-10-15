using System.Collections;
using System.Linq;
using Entities;
using Managers;
using UnityEngine;
using Utility;

namespace BattleSystem.Spells
{
    public abstract class BuffEffect : ScriptableObject, IUpdatable
    {
        [Header("Info")]
        public string Name;
        [TextArea(1, 30)]
        public string ToolTip;
        public Sprite Image;
        
        [Header("Sounds")]
        public AudioEvent CastSound;
        public AudioEvent LooptSound;

        [Header("Effect")]
        public GameObject EffectPrefab;

        [Header("Properties")]
        public float Duration = 1;
        public float TickDelay = .25f;
        
        public Entity Caster { get; protected set; }
        public Entity Target { get; protected set; }
        public BuffEffect EffectDefinition { get; protected set; }
        public float TimeRemaining {  get { return Mathf.Max(0, Duration - _elapsedTime); } }

        protected float _elapsedTime;
        protected float _timeToUpdate;
        protected GameObject _effectObject;

        public virtual void EndEffect()
        {
            if (_effectObject != null)
            {
                Destroy(_effectObject);
            }
            
            Target.Buffs.Remove(this);
            UpdateManager.Instance.Remove(this);
        }

        public virtual void OnUpdate()
        {
            if (TimeRemaining <= 0 || Target.IsDead)
            {
                EndEffect();
                return;
            }

            if (_timeToUpdate <= 0)
            {
                ApplyEffect();
                _timeToUpdate = TickDelay;
            }
            
            _elapsedTime += Time.deltaTime;
            _timeToUpdate -= Time.deltaTime;
        }
        
        public virtual void ApplyTo(Entity target, Entity caster = null)
        {
            var sameEffect = target.Buffs.FirstOrDefault(buff => buff.EffectDefinition == this);
            
            if (sameEffect != null && sameEffect.HandleSameEffect())
            {
                return;
            }

            var newBuff = Instantiate(this);
            newBuff.Target = target;
            newBuff.Caster = caster;
            newBuff.EffectDefinition = this;

            if (EffectPrefab != null)
            {
                newBuff._effectObject = Instantiate(EffectPrefab, target.transform);
            }
            
            target.Buffs.Add(newBuff);
            UpdateManager.Instance.Add(newBuff);
        }
        
        protected abstract void ApplyEffect();

        protected virtual bool HandleSameEffect()
        {
            _elapsedTime = 0;
            return true;
        }
    }
}
