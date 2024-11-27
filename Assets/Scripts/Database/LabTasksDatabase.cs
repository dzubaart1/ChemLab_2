using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Database
{
    public class LabTasksDatabase
    {
        [CanBeNull] private static LabTasksDatabase _singleton;

        public static LabTasksDatabase GetInstance()
        {
            return _singleton ??= new LabTasksDatabase();
        }
        
        public void RemoveAll(ELab lab)
        {
            string[] allFiles = Directory.GetFiles(GetFilePath(lab));
            
            foreach (var file in allFiles)
            {
                File.Delete(file);
            }
        }

        public List<LabTask> ReadAll(ELab lab)
        {
            List<LabTask> res = new List<LabTask>();
            List<string> allTasksFiles = Directory.GetFiles(GetFilePath(lab)).Where(fileName => !fileName.Contains("meta")).ToList();
            
            foreach (var file in allTasksFiles)
            {
                LabTask labTask = JSONSaver.JSONSaver.LoadFromFile<LabTask>(file);
                res.Add(labTask);
            }

            return res;
        }

        public List<LabTask> ReadWhere(Func<LabTask, bool> filter, ELab lab)
        {
            List<LabTask> res = new List<LabTask>();
            List<string> allTasksFiles = Directory.GetFiles(GetFilePath(lab)).Where(fileName => !fileName.Contains("meta")).ToList();
            
            foreach (var file in allTasksFiles)
            {
                LabTask labTask = JSONSaver.JSONSaver.LoadFromFile<LabTask>(file);
                if (filter.Invoke(labTask))
                {
                    res.Add(labTask);
                }
            }

            return res;
        }

        public void Save(LabTask labTask)
        {
            JSONSaver.JSONSaver.SaveToFile(labTask, GetFilePath(labTask.Lab) + GetFileName(labTask.Number));
        }
        
        public string GetFilePath(ELab lab)
        {
            switch (lab)
            {
                case ELab.Lab1:
                    return $"{Application.streamingAssetsPath}/tasksLab1/";
                case ELab.Lab2:
                    return $"{Application.streamingAssetsPath}/tasksLab2/";
                default:
                    Debug.LogError("Can't find labNumber!");
                    return $"{Application.streamingAssetsPath}/tasksLab1/";
            }
        }

        public string GetFileName(int labTaskNumber)
        {
            if (labTaskNumber < 10)
                return $"task_00{labTaskNumber}.txt";
            else if (labTaskNumber < 100) 
                return $"task_0{labTaskNumber}.txt";
            return $"task_{labTaskNumber}.txt";
        }
    }
}