using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect1 : SideEffect
    {
        public int SomeConfig { get; private set; }

        public Effect1()
            : base(ESideEffect.Effect1, ESideEffectTime.StartTask)
        {
        }
        
        public override void ShowInEditor()
        {
            SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", SideEffectTimeType);
            SomeConfig = EditorGUILayout.IntField("Some Config", SomeConfig);
        }

        public override void OnActivated()
        {
        }
    }
}