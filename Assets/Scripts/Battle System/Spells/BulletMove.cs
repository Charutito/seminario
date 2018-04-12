using UnityEngine;


    namespace BattleSystem.Spells
{
	public class BulletMove : MonoBehaviour
	{
		public float speed = 5f;
        public int direction = 1;
		private void Update()
		{
			transform.localPosition += transform.forward* direction * speed * Time.deltaTime;
		}

        public void ChangeDir()
        {
            if (direction ==1)
            {
                direction = -1;
            }
            else
            {
                direction = 1;

            }
        }
	}


}
