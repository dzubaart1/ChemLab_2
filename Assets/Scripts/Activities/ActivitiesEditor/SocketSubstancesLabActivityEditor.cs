using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using Core;
using Database;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Activities.ActivitiesEditor
{
    public class SocketSubstancesLabActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private SocketSubstancesLabActivity _socketSubstancesLabActivity;
        
        public SocketSubstancesLabActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is SocketSubstancesLabActivity handler)
            {
                _socketSubstancesLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_socketSubstancesLabActivity == null)
            {
                return;
            }
            
            _socketSubstancesLabActivity.SocketActivityType = (ESocketActivity)EditorGUILayout.EnumPopup("Socket Activity Type", _socketSubstancesLabActivity.SocketActivityType);
            _socketSubstancesLabActivity.SocketType = (ESocket)EditorGUILayout.EnumPopup("Socket Type", _socketSubstancesLabActivity.SocketType);
            
            EditorGUILayout.LabelField("Substances in Container", EditorStyles.boldLabel);

            for(int i = 0; i < _socketSubstancesLabActivity.LabSubstanceProperties.Length; i++)
            {
                GUILayout.BeginHorizontal();
                
                _socketSubstancesLabActivity.LabSubstanceProperties[i] = GetNewLabSubstanceProperty(_socketSubstancesLabActivity.LabSubstanceProperties[i], i);

                if (GUILayout.Button("Delete"))
                {
                    DeleteSubstanceProperty(i);
                }
                
                GUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button("Add Res Substance Property"))
            {
                AddSubstanceProperty();
            }
        }

        public override EActivity GetActivityType()
        {
            return EActivity.SocketSubstancesActivity;
        }
        
        public void AddSubstanceProperty()
        {
            if (_socketSubstancesLabActivity == null)
            {
                return;
            }
            
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[_socketSubstancesLabActivity.LabSubstanceProperties.Length + 1];
        
            for (int i = 0; i < _socketSubstancesLabActivity.LabSubstanceProperties.Length; i++)
            {
                newArray[i] = _socketSubstancesLabActivity.LabSubstanceProperties[i];
            }

            newArray[^1] = new LabSubstanceProperty();

            _socketSubstancesLabActivity.LabSubstanceProperties = newArray;
        }
        
        public void DeleteSubstanceProperty(int id)
        {
            if (_socketSubstancesLabActivity == null)
            {
                return;
            }
            
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[_socketSubstancesLabActivity.LabSubstanceProperties.Length - 1];
        
            for (int i = 0, j = 0; j < _socketSubstancesLabActivity.LabSubstanceProperties.Length & i < newArray.Length; j++)
            {
                if (j == id)
                {
                    newArray[i++] = _socketSubstancesLabActivity.LabSubstanceProperties[j];   
                }
            }

            _socketSubstancesLabActivity.LabSubstanceProperties = newArray;
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
#endif
    }
}