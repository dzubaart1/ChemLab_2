using UnityEngine;
using Core;
using Database;

namespace Machines
{
    public class WaterDrop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Towel towel = other.transform.GetComponentInChildren<Towel>();

            if (towel == null)
            {
                return;
            }
            
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }
            
            if (gameManager.IsSoundsOn)
            {
                AudioClip a = ResourcesDatabase.ReadSound("Towel");
                AudioSource.PlayClipAtPoint(a, transform.position, 1.0f);
            }

            gameObject.SetActive(false);
        }
    }
}
