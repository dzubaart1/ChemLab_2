using System.Linq;
using BioEngineerLab;
using BioEngineerLab.Activities;
using BioEngineerLab.Substances;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;
using Crafting;
using Database;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class CraftSubstanceActivityEditor : EditorActivity
    {
        [CanBeNull] private CraftSubstanceLabActivity _craftSubstanceActivity;
        private SOLabCraft _soLabCraft;
        
        public CraftSubstanceActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is CraftSubstanceLabActivity handler)
            {
                _craftSubstanceActivity = handler;
            }

            if (_craftSubstanceActivity != null)
            {
                _soLabCraft = ResourcesDatabase.ReadWhereCraft(craft => craft.LabCraft.Equals(_craftSubstanceActivity.LabCraft)).FirstOrDefault();
            }
        }
        
        public override void ShowInEditor()
        {
            if (_craftSubstanceActivity == null)
            {
                return;
            }
            
            _craftSubstanceActivity.Container = (EContainer)EditorGUILayout.EnumPopup("Crafting Container", _craftSubstanceActivity.Container);
            _soLabCraft = EditorGUILayout.ObjectField("Craft", _soLabCraft, typeof(SOLabCraft), true) as SOLabCraft;
            if (_soLabCraft != null)
            {
                _craftSubstanceActivity.LabCraft = new LabCraft(_soLabCraft.LabCraft);
            }
        }

        public override EActivity GetActivityType()
        {
            return EActivity.AddSubstanceActivity;
        }
    }
}