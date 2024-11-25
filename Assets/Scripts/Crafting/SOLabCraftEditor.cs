using System.Linq;
using Core;
using Database;
using JetBrains.Annotations;
using Substances;
using UnityEditor;
using UnityEngine;

namespace Crafting
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SOLabCraft))]
    public class SOLabCraftEditor : Editor
    {
        private SOLabCraft _soLabCraft;
        private SerializedObject _serializedObject;
        
        private void OnEnable()
        {
            _soLabCraft = (SOLabCraft)target;
            _serializedObject = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();
            
            _soLabCraft.LabCraft.CraftType = (ECraft)EditorGUILayout.EnumPopup("Craft Type", _soLabCraft.LabCraft.CraftType);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Substances From", EditorStyles.boldLabel);

            for(int i = 0; i < _soLabCraft.LabCraft.SubstancesFrom.Length; i++)
            {
                GUILayout.BeginHorizontal();
                
                _soLabCraft.LabCraft.SubstancesFrom[i] = GetNewLabSubstanceProperty(_soLabCraft.LabCraft.SubstancesFrom[i], i);

                if (GUILayout.Button("Delete"))
                {
                    _soLabCraft.DeleteFromSubstanceProperty(i);
                }
                
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add From Substance Property"))
            {
                _soLabCraft.AddFromSubstanceProperty();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Substances Res", EditorStyles.boldLabel);
            
            for(int i = 0; i < _soLabCraft.LabCraft.SubstancesRes.Length; i++)
            {
                GUILayout.BeginHorizontal();
                
                _soLabCraft.LabCraft.SubstancesRes[i] = GetNewLabSubstanceProperty(_soLabCraft.LabCraft.SubstancesRes[i], i);
                
                if (GUILayout.Button("Delete"))
                {
                    _soLabCraft.DeleteResSubstanceProperty(i);
                }
                
                GUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button("Add Res Substance Property"))
            {
                _soLabCraft.AddResSubstanceProperty();
            }
            
            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_soLabCraft);
        }

        [CanBeNull]
        private LabSubstanceProperty GetNewLabSubstanceProperty(LabSubstanceProperty target, int id)
        {
            SOLabSubstanceProperty labSubstanceProperty = ResourcesDatabase.ReadWhereSubstanceProperties(
                    substanceProperty =>
                        substanceProperty.LabSubstanceProperty.Equals(target))
                .FirstOrDefault();
            
            labSubstanceProperty = EditorGUILayout.ObjectField($"Substance {id}", labSubstanceProperty, typeof(SOLabSubstanceProperty), true) as SOLabSubstanceProperty;
            
            if (labSubstanceProperty == null)
            {
                return null;
            }

            return new LabSubstanceProperty(labSubstanceProperty.LabSubstanceProperty);
        }
    }
#endif
}