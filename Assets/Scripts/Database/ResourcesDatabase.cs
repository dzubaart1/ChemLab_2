using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Substances;
using BioEngineerLab.Tasks;
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

        public static void LoadAllTaskFromDataBase()
        {
            List<SOLabTask> scriptableObjects =
                Resources.LoadAll<SOLabTask>("Tasks").ToList();

            foreach (var so in scriptableObjects)
            {
                so.LoadHandlerByTaskNumber();
            }
        }
        
        public static void SaveAllTaskToDataBase()
        {
            LabTasksDatabase.GetInstance().RemoveAll();
            
            List<SOLabTask> scriptableObjects =
                Resources.LoadAll<SOLabTask>("Tasks").ToList();

            foreach (var so in scriptableObjects)
            {
                so.SaveHandlerByTaskNumber();
            }
        }
    }
}