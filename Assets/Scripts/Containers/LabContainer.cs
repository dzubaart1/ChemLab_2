using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using Gameplay;
using JetBrains.Annotations;
using Saveables;
using UnityEngine;

namespace Containers
{
    public class LabContainer : MonoBehaviour, ISaveableContainer, ISideEffectActivator
    {
        [Serializable]
        private struct MeshRendererConfig
        {
            public ESubstanceLayer Layer;
            public MeshRenderer MeshRenderer;
        }
        
        private class SavedData
        {
            public Anchor Anchor;
            public bool IsAnimating;
            public LabSubstance[] Substances = new LabSubstance[MAX_SUBSTANCE_COUNT];
            public EContainer ContainerType;
        }
        
        public const int MAX_SUBSTANCE_COUNT = 3;

        [Header("Container Configs")]
        [SerializeField] private SOLabSubstanceProperty reagentsLabSubstanceProperty;
        [SerializeField] private float _maxVolume = 9000;
        [SerializeField] private float _containerWeight;
        [SerializeField] private EContainer _containerType;
        [SerializeField] private bool _isWeightableContainer;
        [SerializeField] private bool _isSpoonContainer;

        [Space]
        [Header("Meshes")]
        [SerializeField] private MeshRendererConfig[] _meshRendererConfigs;

        private Anchor Anchor { get; set; }
        
        public float MaxVolume
        {
            get
            {
                return _maxVolume;
            }
        }
        
        public bool IsDirty { get; private set; }

        public IReadOnlyCollection<LabSubstance> Substances
        {
            get
            {
                return _substances;
            }
        }

        public EContainer ContainerType
        {
            get
            {
                return _containerType;
            }
        }

        public bool IsWeightableContainer
        {
            get
            {
                return _isWeightableContainer;
            }
        }

        public bool IsSpoonContainer
        {
            get
            {
                return _isSpoonContainer;
            }
        }

        
        private LabSubstance[] _substances = new LabSubstance[MAX_SUBSTANCE_COUNT];
        private SavedData _savedData = new SavedData();

        private void Start()
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
            
            gameManager.CurrentBaseLocalManager.AddSaveableContainer(this);
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            
            UpdateView();
        }

        public void ChangeMaxVolume(float value)
        {
            if (GetSubstancesWeight() > value)
            {
                return;
            }

            _maxVolume = value;
        }

        public void ChangeContainerType(EContainer value)
        {
            _containerType = value;
        }
        
        public float GetSubstancesWeight()
        {
            float sumWeight = 0;
            foreach (var substance in _substances)
            {
                if (substance == null)
                {
                    continue;
                }
                
                sumWeight += substance.Weight;
            }

            return sumWeight;
        }

        public float GetContainerWeight()
        {
            return _containerWeight;
        }

        public float GetAvailableWeight()
        {
            return _maxVolume - GetSubstancesWeight();
        }

        public int GetSubstancesCount()
        {
            int res = _substances[0] != null ? 1 : 0;
            res += _substances[1] != null ? 1 : 0;
            res += _substances[2] != null ? 1 : 0;

            return res;
        }

        [CanBeNull]
        public LabSubstance GetTopSubstance()
        {
            for (int i = 0; i < MAX_SUBSTANCE_COUNT; i++)
            {
                if (_substances[i] != null)
                {
                    return _substances[i];
                }
            }

            return null;
        }

        public void PutSubstance(LabSubstance labSubstance)
        {
            switch (labSubstance.SubstanceProperty.SubstanceLayer)
            {
                case ESubstanceLayer.Top:
                    _substances[0] = labSubstance;
                    break;
                case ESubstanceLayer.Middle:
                    _substances[1] = labSubstance;
                    break;
                case ESubstanceLayer.Bottom:
                    _substances[2] = labSubstance;
                    break;
            }

            IsDirty = true;
            UpdateView();
        }

        public void DeleteSubstanceByLayer(ESubstanceLayer layer)
        {
            switch (layer)
            {
                case ESubstanceLayer.Top:
                    _substances[0] = null;
                    break;
                case ESubstanceLayer.Middle:
                    _substances[1] = null;
                    break;
                case ESubstanceLayer.Bottom:
                    _substances[2] = null;
                    break;
            }
            
            UpdateView();
        }

        public void ClearContainer()
        {
            _substances[0] = null;
            _substances[1] = null;
            _substances[2] = null;
            
            UpdateView();
        }

        public void UpdateSubstances(LabSubstance[] substances)
        {
            if (substances.Length != MAX_SUBSTANCE_COUNT)
            {
                return;
            }
            
            _substances[0] = substances[0];
            _substances[1] = substances[1];
            _substances[2] = substances[2];
            
            IsDirty = true;
            UpdateView();
        }
        
        public void PrintContainerInfo()
        {
            string printString = "";
            printString += $"Name:{gameObject.name}\n"; 
            printString += "Substances:\n";
            
            for(int i = 0; i < _substances.Length; i++)
            {
                if (_substances[i] != null)
                {
                    printString += $"{i}. {_substances[i].SubstanceProperty.SubstanceName.ToString()}\n";
                }
                else
                {
                    printString += $"{i}. NULL";
                }
            }

            printString += "Weights:\n";
            printString += $"Container weight {_containerWeight}\n";
            printString += $"Substances sum weight {GetSubstancesWeight()}\n";

            Debug.Log($"<color=yellow>{printString}</color>");
        }

        public LabSubstance GetSubstanceByLayer(ESubstanceLayer layer)
        {
            return _substances[(int)layer];
        }
        
        private void UpdateView()
        {
            for (int i = 0; i < _substances.Length; i++)
            {
                if (!TryGetMeshRendererByLayer((ESubstanceLayer)i, out MeshRenderer meshRenderer))
                {
                    continue;
                }

                if (_substances[i] == null)
                {
                    meshRenderer.enabled = false;
                }
                else
                {
                    meshRenderer.enabled = true;
                    meshRenderer.material.color = _substances[i].GetColor();
                }
            }
        }

        public IReadOnlyCollection<LabSubstanceProperty> GetSubstanceProperties()
        {
            return _substances.Where(temp => temp != null).Select(temp => temp.SubstanceProperty).ToList();
        }

        private bool TryGetMeshRendererByLayer(ESubstanceLayer layer, out MeshRenderer meshRenderer)
        {
            meshRenderer = null;
            
            foreach (var config in _meshRendererConfigs)
            {
                if (config.Layer == layer)
                {
                    meshRenderer = config.MeshRenderer;
                    return true;
                }
            }

            return false;
        }

        public void Save()
        {
            _savedData.Substances = new LabSubstance[Substances.Count];
            
            for(int i = 0; i < Substances.Count; i++)
            {
                if(GetSubstanceByLayer((ESubstanceLayer)i) is not null)
                    _savedData.Substances[i] = new LabSubstance(GetSubstanceByLayer((ESubstanceLayer)i));
            }
            
            _savedData.ContainerType = ContainerType;
        }

        public void PutSavedContainerType()
        {
            UpdateSubstances(_savedData.Substances);
        }

        public void PutSavedSubstances()
        {
            ChangeContainerType(_savedData.ContainerType);
        }

        public void LoadAnchor()
        {
            if (_savedData.Anchor == null & Anchor == null)
            {
                AnimateAnchor(_savedData.IsAnimating);
            }

            if (_savedData.Anchor == null & Anchor != null)
            {
                ReleaseAnchor();
            }

            if (_savedData.Anchor != null & Anchor == null)
            {
                PutAnchor(_savedData.Anchor);
                AnimateAnchor(_savedData.IsAnimating);
            }

            if (_savedData.Anchor != null & Anchor != null)
            {
                PutAnchor(_savedData.Anchor);
                AnimateAnchor(_savedData.IsAnimating);
            }
        }
        
        public void PutAnchor(Anchor anchor)
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
            
            if (Anchor != null)
            {
                return;
            }
            
            Anchor = anchor;
            Anchor.TogglePhysics(false);
            Anchor.transform.parent = transform;
            Anchor.transform.localPosition = new Vector3(0, 0.01f, 0);
            Anchor.transform.rotation = Quaternion.identity;
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new AnchorLabActivity(ContainerType));
        }

        public void AnimateAnchor(bool value)
        {
            if (Anchor == null)
            {
                return;
            }
            
            Anchor.ToggleAnimate(value);
            _savedData.IsAnimating = value;
        }

        private void ReleaseAnchor()
        {
            if (Anchor == null)
            {
                return;
            }
            
            Anchor.transform.parent = null;
            Anchor.TogglePhysics(true);
            
            Anchor = null;
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (reagentsLabSubstanceProperty == null)
            {
                return;
            }
            
            if (sideEffect is not AddReagentsLabSideEffect addReagentsLabSideEffect)
            {
                return;
            }

            if (addReagentsLabSideEffect.LabSubstanceProperty.Equals(reagentsLabSubstanceProperty.LabSubstanceProperty))
            {
                PutSubstance(new LabSubstance(reagentsLabSubstanceProperty.LabSubstanceProperty, addReagentsLabSideEffect.Weight));
            }
        }
    }
}
