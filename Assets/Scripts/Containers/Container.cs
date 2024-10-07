using System;
using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class Container : MonoBehaviour
    {
        [Serializable]
        private struct MeshRendererConfig
        {
            public ESubstanceLayer Layer;
            public MeshRenderer MeshRenderer;
        }
        
        public const int MAX_SUBSTANCE_COUNT = 3;

        [Header("Container Configs")]
        [SerializeField] private float _maxVolume = 9000;
        [SerializeField] private float _containerWeight;
        [SerializeField] private EContainer _containerType;
        [SerializeField] private bool _isReagentsContainer;
        [SerializeField] private bool _isWeightableContainer;
        [SerializeField] private bool _isMixableContainer;
        [SerializeField] private bool _isSpoonContainer;

        [Header("Meshes")]
        [SerializeField] private MeshRendererConfig[] _meshRendererConfigs;

        public bool IsDirty { get; private set; }

        public IReadOnlyCollection<Substance> Substances
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

        public bool IsReagentsContainer
        {
            get
            {
                return _isReagentsContainer;
            }
        }

        public bool IsWeightableContainer
        {
            get
            {
                return _isWeightableContainer;
            }
        }

        public bool IsMixableContainer
        {
            get
            {
                return _isMixableContainer;
            }
        }

        public bool IsSpoonContainer
        {
            get
            {
                return _isSpoonContainer;
            }
        }

        private ContainerService _containerService;
        
        private Substance[] _substances = new Substance[MAX_SUBSTANCE_COUNT];
        
        private void Start()
        {
            _containerService = Engine.GetService<ContainerService>();
            _containerService.RegisterContainer(this);
            
            UpdateView();
        }
        
        public float GetSubstancesWeight()
        {
            float sumWeight = 0;
            foreach (var substance in _substances)
            {
                sumWeight += substance.Weight;
            }

            return sumWeight;
        }

        public float GetAvailableWeight()
        {
            return _maxVolume - GetSubstancesWeight();
        }

        public void PutSubstance(Substance substance)
        {
            switch (substance.SubstanceProperty.SubstanceLayer)
            {
                case ESubstanceLayer.Top:
                    _substances[0] = substance;
                    break;
                case ESubstanceLayer.Middle:
                    _substances[1] = substance;
                    break;
                case ESubstanceLayer.Bottom:
                    _substances[2] = substance;
                    break;
            }

            IsDirty = true;
            UpdateView();
        }

        public void UpdateSubstances(Substance[] substances)
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
                printString += $"{i}. {_substances[i].SubstanceProperty.GetSubstanceName()}\n";
            }

            printString += "Weights:\n";
            printString += $"Container weight {_containerWeight}\n";
            printString += $"Substances sum weight {GetSubstancesWeight()}\n";

            Debug.Log($"<color=yellow>{printString}</color>");
        }

        public Substance GetSubstanceByLayer(ESubstanceLayer layer)
        {
            return _substances[(int)layer];
        }
        
        private void UpdateView()
        {
            for (int i = 0; i < _substances.Length; i++)
            {
                if (!TryGetMeshRendererByLayer((ESubstanceLayer)i, out MeshRenderer meshRenderer))
                {
                    if (_substances[i] == null)
                    {
                        meshRenderer.enabled = false;
                    }
                    else
                    {
                        meshRenderer.enabled = true;
                        meshRenderer.material.color = _substances[i].SubstanceProperty.Color;
                    }
                }
            }
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
    }
}
