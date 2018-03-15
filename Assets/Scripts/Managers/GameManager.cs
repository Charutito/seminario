using System.Collections;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return (this == Instance) ? StartCoroutine(enumerator) : null;
        }
    }
}


