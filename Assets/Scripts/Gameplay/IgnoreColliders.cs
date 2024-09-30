using System.Collections.Generic;
using UnityEngine;

namespace BioEngineerLab.Gameplay
{
    public class IgnoreColliders : MonoBehaviour
    {
        public List<Collider> CollidersToIgnore;
        private Collider[] _thisColliders;
        
        private void Start()
        {
            _thisColliders = GetComponents<Collider>();
            var thisCol = GetComponent<Collider>();

            if (CollidersToIgnore is null)
            {
                return;
            }
            
            foreach (var thisCollider in _thisColliders)
            {
                foreach (var col in CollidersToIgnore)
                {
                    if (col && col.enabled)
                    {
                        Physics.IgnoreCollision(thisCollider, col, true);
                    }
                }
            }
        }
    }
}