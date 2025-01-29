using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private XRDirectInteractor _leftDirectInteractor;
        [SerializeField] private XRDirectInteractor _rightDirectInteractor;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void ReleaseAllGrabbables()
        {
            for (var i = _leftDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                _leftDirectInteractor.interactionManager.SelectCancel(_leftDirectInteractor, _leftDirectInteractor.interactablesSelected[i]);
            }
            
            for (var i = _rightDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                _rightDirectInteractor.interactionManager.SelectCancel(_rightDirectInteractor, _rightDirectInteractor.interactablesSelected[i]);
            }
        }
    }
}