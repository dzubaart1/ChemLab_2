using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Tasks.Activities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BioEngineerLab.Machines
{
    public class ExpTabletMachine : MonoBehaviour
    {
        [SerializeField] private List <LabContainer> _labContainers = new List<LabContainer>();

        private SaveService _saveService;
        private TasksService _tasksService;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (LabContainer labContainer in _labContainers)
            {
                if (labContainer.GetSubstancesCount() == 0)
                    return;
            }

            _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.ExpTabletMachine));
        }

    }
}
