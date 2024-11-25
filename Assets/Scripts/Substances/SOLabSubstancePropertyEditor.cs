using Core;
using UnityEditor;
using UnityEngine;

namespace Substances
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SOLabSubstanceProperty))]
    public class SOLabSubstancePropertyEditor : Editor
    {
        private SOLabSubstanceProperty _soSubstanceProperty;
        private SerializedObject _serializedObject;
        
        private void OnEnable()
        {
            _soSubstanceProperty = (SOLabSubstanceProperty)target;
            _serializedObject = new SerializedObject(target);
        }
        
        public override void OnInspectorGUI()
        {
            _serializedObject.Update();
            
            Color substanceColor = new Color(_soSubstanceProperty.LabSubstanceProperty.SubstanceColorR,
                _soSubstanceProperty.LabSubstanceProperty.SubstanceColorG,
                _soSubstanceProperty.LabSubstanceProperty.SubstanceColorB,
                _soSubstanceProperty.LabSubstanceProperty.SubstanceColorA);
            
            _soSubstanceProperty.LabSubstanceProperty.SubstanceName = EditorGUILayout.TextField("Substance Name", _soSubstanceProperty.LabSubstanceProperty.SubstanceName);
            _soSubstanceProperty.LabSubstanceProperty.SubstanceLayer = (ESubstanceLayer)EditorGUILayout.EnumPopup("Substance Layer", _soSubstanceProperty.LabSubstanceProperty.SubstanceLayer);
            _soSubstanceProperty.LabSubstanceProperty.SubstanceMode = (ESubstanceMode)EditorGUILayout.EnumPopup("Substance Mode", _soSubstanceProperty.LabSubstanceProperty.SubstanceMode);
            
            substanceColor = EditorGUILayout.ColorField("Substance Color", substanceColor);

            _soSubstanceProperty.LabSubstanceProperty.SubstanceColorR = substanceColor.r;
            _soSubstanceProperty.LabSubstanceProperty.SubstanceColorG = substanceColor.g;
            _soSubstanceProperty.LabSubstanceProperty.SubstanceColorB = substanceColor.b;
            _soSubstanceProperty.LabSubstanceProperty.SubstanceColorA = substanceColor.a;
            
            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_soSubstanceProperty);
        }
    }
#endif
}