using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;
using Mechanics;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class CapChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CleaningSurface"))
            {
                GameManager gameManager = GameManager.Instance;
                if (gameManager == null)
                {
                    return;
                }

                if (gameManager.CurrentBaseLocalManager == null)
                {
                    return;
                }
                
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
            }
        }
    }
}