using BioEngineerLab.Tasks;
using Core;
using UnityEngine;

namespace UI.TabletUI
{
    public abstract class BaseTabletPanel : MonoBehaviour
    {
        public TabletUI TabletUI { get; set; }
        
        public TabletUI.ETabletUIPanel TabletPanelType;
        public abstract void SetTaskToShow(LabTask task);
        public abstract void SetLabToShow(ELab lab);
    }
}