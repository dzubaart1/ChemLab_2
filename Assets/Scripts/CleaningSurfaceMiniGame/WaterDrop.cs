using UnityEngine;

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

            gameObject.SetActive(false);
        }
    }
}
