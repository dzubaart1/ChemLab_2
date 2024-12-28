using BioEngineerLab.Tasks;
using UnityEngine;

namespace UI.TabletUI
{
    public abstract class BaseTabletPanel : MonoBehaviour
    {
        public TabletUI.ETabletUIPanel TabletPanelType;
        public abstract void SetTaskToShow(LabTask task);
    }
}