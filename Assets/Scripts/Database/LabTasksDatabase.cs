using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public static class LabTasksDatabase
    {
        public static void RemoveAll(ELab lab)
        {
            string[] allFiles = Directory.GetFiles(GetFilePath(lab));
            
            foreach (var file in allFiles)
            {
                File.Delete(file);
            }
        }

        public static List<LabTask> ReadAll(ELab lab)
        {
            BetterStreamingAssets.Initialize();

            List<LabTask> res = new List<LabTask>();

            try
            {
                string folderName = GetFolderName(lab);
                
                string[] allFiles = BetterStreamingAssets.GetFiles(folderName, "*.txt", SearchOption.AllDirectories);

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                
                foreach (var file in allFiles)
                {
                    string fileContent = BetterStreamingAssets.ReadAllText(file);
                    
                    LabTask labTask = JsonConvert.DeserializeObject<LabTask>(fileContent, settings);

                    res.Add(labTask);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading files from {lab}: {e.Message}");
            }

            return res;
        }

        public static List<LabTask> ReadWhere(Func<LabTask, bool> filter, ELab lab)
        {
            List<LabTask> res = new List<LabTask>();
            List<string> allTasksFiles = Directory.GetFiles(GetFilePath(lab)).Where(fileName => !fileName.Contains("meta")).ToList();
            
            foreach (var file in allTasksFiles)
            {
                LabTask labTask = JSONSavers.JSONSaver.LoadFromFile<LabTask>(file);
                if (filter.Invoke(labTask))
                {
                    res.Add(labTask);
                }
            }

            return res;
        }

        public static void Save(LabTask labTask)
        {
            JSONSavers.JSONSaver.SaveToFile(labTask, GetFilePath(labTask.Lab) + GetFileName(labTask.Number));
        }

        public static string GetFolderName(ELab lab)
        {
            switch (lab)
            {
                case ELab.Lab1:
                    return "tasksforlab1";
                case ELab.Lab2:
                    return "tasksforlab2";
                case ELab.Lab3:
                    return "tasksforlab3";
                default:
                    Debug.LogError("Can't find lab!");
                    return "tasksforlab1";
            }
        }
        
        public static string GetFilePath(ELab lab)
        {
            switch (lab)
            {
                case ELab.Lab1:
                    return $"{Application.streamingAssetsPath}/{GetFolderName(lab)}/";
                case ELab.Lab2:
                    return $"{Application.streamingAssetsPath}/{GetFolderName(lab)}/";
                case ELab.Lab3:
                    return $"{Application.streamingAssetsPath}/{GetFolderName(lab)}/";
                default:
                    Debug.LogError("Can't find labNumber!");
                    return $"{Application.streamingAssetsPath}/tasksLab1/";
            }
        }

        public static string GetFileName(int labTaskNumber)
        {
            if (labTaskNumber < 10)
                return $"task_00{labTaskNumber}.txt";
            else if (labTaskNumber < 100) 
                return $"task_0{labTaskNumber}.txt";
            return $"task_{labTaskNumber}.txt";
        }
    }
}