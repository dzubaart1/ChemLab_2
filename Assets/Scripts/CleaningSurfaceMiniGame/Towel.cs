using UnityEngine;

namespace Machines
{
    public class Towel : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            WaterDrop waterDrop = other.transform.GetComponentInChildren<WaterDrop>();

            if (waterDrop == null)
            {
                return;
            }
            
            other.gameObject.SetActive(false);
        }
    }
}