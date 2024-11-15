using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioEngineerLab.JSON;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;
using JetBrains.Annotations;
using UnityEngine;

namespace Database
{
    public class LabTasksDatabase : IDatabase<LabTask>
    {
        [CanBeNull] private static LabTasksDatabase _singleton;

        public static LabTasksDatabase GetInstance()
        {
            return _singleton ??= new LabTasksDatabase();
        }
        
        public void Add(LabTask item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            string[] allFiles = Directory.GetFiles(GetFilePath());
            
            foreach (var file in allFiles)
            {
                File.Delete(file);
            }
        }

        public List<LabTask> ReadAll()
        {
            List<LabTask> res = new List<LabTask>();
            List<string> allTasksFiles = Directory.GetFiles(GetFilePath()).Where(fileName => !fileName.Contains("meta")).ToList();
            
            foreach (var file in allTasksFiles)
            {
                LabTask labTask = JSONSaver.LoadFromFile<LabTask>(file);
                res.Add(labTask);
            }

            return res;
        }

        public List<LabTask> ReadWhere(Func<LabTask, bool> filter)
        {
            List<LabTask> res = new List<LabTask>();
            List<string> allTasksFiles = Directory.GetFiles(GetFilePath()).Where(fileName => !fileName.Contains("meta")).ToList();
            
            foreach (var file in allTasksFiles)
            {
                LabTask labTask = JSONSaver.LoadFromFile<LabTask>(file);
                if (filter.Invoke(labTask))
                {
                    res.Add(labTask);
                }
            }

            return res;
        }

        public void Save(LabTask labTask)
        {
            JSONSaver.SaveToFile(labTask, GetFilePath() + GetFileName(labTask));
        }
        
        public string GetFilePath()
        {
            return $"{Application.streamingAssetsPath}/tasks/";
        }

        public string GetFileName(LabTask labTask)
        {
            if (labTask.Number < 10)
                return $"task_00{labTask.Number}.txt";
            else if (labTask.Number < 100) 
                return $"task_0{labTask.Number}.txt";
            return $"task_{labTask.Number}.txt";
        }
    }
}