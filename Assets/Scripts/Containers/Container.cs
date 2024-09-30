using System.Collections.Generic;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class Container : MonoBehaviour
    {
        public IReadOnlyCollection<Substance> SubstancesList => _substancesList;
        private List<Substance> _substancesList;

        public float MaxWeight = 9000;
        public float ContainerWeight;
        public ContainerType ContainerType;
        public bool IsDirty;
        public MeshRenderer SubstancePrefabRenderer;

        [Header("Base Container Toggles")]
        public bool IsReagentsContainer;
        public bool IsWeightableContainer;
        public bool IsMixableContainer;
        public bool IsSpoonContainer;

        public const int MAX_SUBSTANCE_COUNT = 5;

        private void Awake()
        {
            _substancesList = new List<Substance>(MAX_SUBSTANCE_COUNT);
        }

        private void Start()
        {
            if (!SubstancePrefabRenderer)
            {
                return;
            }
            
            SubstancePrefabRenderer.enabled = false;
        }

        public float GetSumSubstancesWeight()
        {
            float sumWeight = ContainerWeight;
            foreach (var substance in _substancesList)
            {
                sumWeight += substance.Weight;
            }

            return sumWeight;
        }

        public float GetAvailableWeight()
        {
            return MaxWeight - GetSumSubstancesWeight();
        }

        public Substance RemoveLastSubstance()
        {
            if (_substancesList.Count == 0)
            {
                return null;
            }
            
            var res = _substancesList[^1];
            _substancesList.RemoveAt(_substancesList.Count - 1);
            UpdateView();
            return res;
        }
        
        public Substance PeekLastSubstance()
        {
            if (_substancesList.Count == 0)
            {
                return null;
            }
            
            return _substancesList[^1];
        }

        public void AddSubstance(Substance substance)
        {
            _substancesList.Add(substance);
            IsDirty = true;
            UpdateView();
        }

        public void UpdateSubstancesList(List<Substance> substances)
        {
            _substancesList.Clear();
            _substancesList.AddRange(substances);
            UpdateView();
        }
        
        public void PrintContainerInfo()
        {
            string printString = "";
            printString += $"Name:{gameObject.name}\n"; 
            printString += "Substances Stack:\n";

            int i = 0;
            foreach (var substance in _substancesList)
            {
                printString += $"{++i}. {substance.SubstanceProperty.GetSubstanceName()}\n";
            }

            printString += "Weights:\n";
            printString += $"Container weight {ContainerWeight}\n";
            printString += $"Substances sum weight {GetSumSubstancesWeight()}\n";

            Debug.Log($"<color=yellow>{printString}</color>");
        }
        
        private void UpdateView()
        {
            if (IsReagentsContainer)
            {
                return;
            }
            
            if(_substancesList.Count == 0)
            {
                SubstancePrefabRenderer.enabled = false;
                return;
            }
            
            SubstancePrefabRenderer.enabled = true;
        }
    }
}
