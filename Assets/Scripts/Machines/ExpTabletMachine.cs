using System.Collections.Generic;
using Activities;
using Containers;
using Core;
using Core.Services;
using UnityEngine;

namespace Machines
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
