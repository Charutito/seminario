using System.Collections.Generic;
using System.Linq;
using GameUtils;
using Utility;

namespace Managers
{
    public class UpdateManager : SingletonObject<UpdateManager>
    {
        private HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
        private HashSet<IUpdatable> _updatesToRemove = new HashSet<IUpdatable>();
        private HashSet<IUpdatable> _updatesToAdd = new HashSet<IUpdatable>();

        public void Add(IUpdatable updatable)
        {
            _updatesToAdd.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            _updatesToRemove.Add(updatable);
        }

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.OnUpdate();
            }
            
            if (_updatesToAdd.Any())
            {
                foreach (var updatable in _updatesToAdd)
                {
                    _updatables.Add(updatable);
                }
                _updatesToAdd.Clear();
            }

            if (_updatesToRemove.Any())
            {
                foreach (var updatable in _updatesToRemove)
                {
                    _updatables.Remove(updatable);
                }
                _updatesToRemove.Clear();
            }
        }
    }
}
