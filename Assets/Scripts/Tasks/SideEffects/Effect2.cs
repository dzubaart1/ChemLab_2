using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect2 : SideEffect
    {
        public int SomeConfig2;

        public Effect2()
            : base(ESideEffect.Effect2, ESideEffectTime.StartTask)
        {
        }
        
        public override void ShowInEditor()
        {
            SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", SideEffectTimeType);
            SomeConfig2 = EditorGUILayout.IntField("Some Config 2", SomeConfig2);
        }

        public override void OnActivated()
        {
        }
    }
}