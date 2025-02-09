using Machines;
using UI.TabletUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private HandsChanger _handsChanger;
        [SerializeField] private TabletUI _tabletUI;
        
        [Space]
        [Header("Interactors")]
        [SerializeField] private XRDirectInteractor _leftDirectInteractor;
        [SerializeField] private XRDirectInteractor _rightDirectInteractor;
        [SerializeField] private XRRayInteractor _leftRayInteractor;
        [SerializeField] private XRRayInteractor _rightRayInteractor;

        public HandsChanger HandsChanger => _handsChanger;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _rightDirectInteractor.selectEntered.AddListener(OnRightHandSelected);
            _leftDirectInteractor.selectEntered.AddListener(OnLeftHandSelected);
            
            _rightDirectInteractor.selectExited.AddListener(OnRightHandExited);
            _leftDirectInteractor.selectExited.AddListener(OnLeftHandExited);
        }

        private void OnDisable()
        {
            _rightDirectInteractor.selectEntered.RemoveListener(OnRightHandSelected);
            _leftDirectInteractor.selectEntered.RemoveListener(OnLeftHandSelected);
            
            _rightDirectInteractor.selectExited.RemoveListener(OnRightHandExited);
            _leftDirectInteractor.selectExited.RemoveListener(OnLeftHandExited);
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

        public void Init()
        {
            _handsChanger.Init();
            _tabletUI.Init();
        }

        private void OnRightHandSelected(SelectEnterEventArgs args)
        {
            _rightRayInteractor.enableUIInteraction = false;
        }
        
        private void OnRightHandExited(SelectExitEventArgs args)
        {
            _rightRayInteractor.enableUIInteraction = true;
        }

        private void OnLeftHandSelected(SelectEnterEventArgs args)
        {
            _leftRayInteractor.enableUIInteraction = false;
        }

        private void OnLeftHandExited(SelectExitEventArgs args)
        {
            _leftRayInteractor.enableUIInteraction = true;
        }
    }
}