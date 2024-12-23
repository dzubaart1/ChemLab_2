using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Tasks;
using Core;
using Crafting;
using UnityEngine;

namespace Database
{
    public static class ResourcesDatabase
    {
        public static List<SOLabSubstanceProperty> ReadAllSubstanceProperties()
        {
            return Resources.LoadAll<SOLabSubstanceProperty>("Substances").ToList();
        }

        public static List<SOLabSubstanceProperty> ReadWhereSubstanceProperties(Func<SOLabSubstanceProperty, bool> filter)
        {
            return Resources.LoadAll<SOLabSubstanceProperty>("Substances")
                .Where(filter.Invoke).ToList();
        }
        
        public static List<SOLabCraft> ReadAllCraft()
        {
            return Resources.LoadAll<SOLabCraft>("Crafts").ToList();
        }

        public static List<SOLabCraft> ReadWhereCraft(Func<SOLabCraft, bool> filter)
        {
            return Resources.LoadAll<SOLabCraft>("Crafts")
                .Where(filter.Invoke).ToList();
        }

        public static void LoadAllTaskFromDataBase(ELab lab)
        {
            List<SOLabTask> scriptableObjects =
                Resources.LoadAll<SOLabTask>(GetResourcesNameByLabNumber(lab)).ToList();

            foreach (var so in scriptableObjects)
            {
                so.LoadHandlerByTaskNumber();
            }
        }
        
        public static void SaveAllTaskToDataBase(ELab lab)
        {
            LabTasksDatabase.RemoveAll(lab);
            
            List<SOLabTask> scriptableObjects =
                Resources.LoadAll<SOLabTask>(GetResourcesNameByLabNumber(lab)).ToList();

            foreach (var so in scriptableObjects)
            {
                so.SaveHandlerByTaskNumber();
            }
        }

        private static string GetResourcesNameByLabNumber(ELab lab)
        {
            switch (lab)
            {
                case ELab.Lab1:
                    return "TasksLab1";
                case ELab.Lab2:
                    return "TasksLab2";
                case ELab.Lab3:
                    return "TasksLab3";
                default:
                    Debug.LogError("Can't find labnumber!");
                    return "TasksLab1";
            }
        }
    }
}