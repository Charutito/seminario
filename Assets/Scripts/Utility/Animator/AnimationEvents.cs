using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
	public sealed class AnimationEvents : MonoBehaviour
	{
		[Serializable]
		private sealed class EventHandlerInfo
		{
			[SerializeField] public string name;
			[SerializeField] public UnityEvent action;
		}

		[SerializeField]
		private List<EventHandlerInfo> handlers;

		private Dictionary<string, EventHandlerInfo> handlerLookup;

		public void TriggerEvent(string evt)
		{
			handlerLookup = handlerLookup ?? handlers.ToDictionary(eventInfo => eventInfo.name);

			EventHandlerInfo info;

			if (handlerLookup.TryGetValue(evt, out info))
			{
				info.action.Invoke();
			}
		}
	}
}