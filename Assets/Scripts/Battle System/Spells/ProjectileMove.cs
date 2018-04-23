using UnityEngine;


    namespace BattleSystem.Spells
{
	public class ProjectileMove : MonoBehaviour
	{
		private SpellBehaviour _behaviour;
		private int _direction = 1;
		private float _speed;
		
		private void Start()
		{
			_behaviour = GetComponent<SpellBehaviour>();
			_speed = _behaviour.Definition.Speed;
		}
		
		private void Update()
		{
			transform.localPosition += transform.forward * _direction * _speed * Time.deltaTime;
		}

        public void ChangeDir()
        {
	        _direction = -_direction;
        }
	}
}
