using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    [DisallowMultipleComponent]
    public class SaveGUID : MonoBehaviour
    {
        public string GameObjectId = "";
    }
}
