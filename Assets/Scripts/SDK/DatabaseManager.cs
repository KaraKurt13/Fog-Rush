using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class DatabaseManager
{
    public static void LoadLevelData(string userId, Action<Dictionary<int,LevelData>> onLoaded = null)
    {
        var _database = FirebaseDatabase.DefaultInstance.RootReference;
        _database.Child("users").Child(userId).Child("levels")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Exists)
                    {
                        var levels = new Dictionary<int, LevelData>();
                        foreach (var child in snapshot.Children)
                        {
                            if (int.TryParse(child.Key, out int levelId))
                            {
                                var json = child.GetRawJsonValue();
                                LevelData levelData = JsonConvert.DeserializeObject<LevelData>(json);
                                levels.Add(levelId, levelData);
                            }
                        }
                        onLoaded?.Invoke(levels);
                    }
                    else
                    {
                        Debug.Log("No data found for user. Initializing...");
                        InitUserData(userId, () =>
                        {
                            LoadLevelData(userId, onLoaded);
                        });
                    }
                }
                else
                {
                    Debug.LogError("Failed to load levels: " + task.Exception);
                    onLoaded?.Invoke(new Dictionary<int, LevelData>());
                }
            });
    }

    public static void SaveLevelData(string userId, Dictionary<int,LevelData> levelData)
    {

    }

    public static void InitUserData(string userId, Action onInitialize = null)
    {
        DatabaseReference configRef = FirebaseDatabase.DefaultInstance.RootReference.Child("config");
        configRef.GetValueAsync().ContinueWithOnMainThread(configTask =>
        {
            if (configTask.IsCompleted)
            {
                DataSnapshot configSnapshot = configTask.Result;
                int totalLevels = int.Parse(configSnapshot.Child("totalLevels").Value.ToString());
                DatabaseReference userRef = FirebaseDatabase.DefaultInstance
                   .RootReference.Child("users").Child(userId);

                userRef.GetValueAsync().ContinueWithOnMainThread(userTask => 
                {
                    DataSnapshot userSnapshot = userTask.Result;
                    if (!userSnapshot.Exists)
                    {
                        var levels = new Dictionary<int, LevelData>();
                        for (int i = 1; i <= totalLevels; i++)
                        {
                            var levelData = new LevelData(0, 0, false);
                            levels.Add(i, levelData);
                        }

                        levels[1].IsUnlocked = true;

                        var levelsData = levels.ToDictionary(
                            kvp => kvp.Key.ToString(),
                            kvp => kvp.Value
                        );
                        var json = JsonConvert.SerializeObject(levelsData);

                        userRef.Child("levels").SetRawJsonValueAsync(json).ContinueWithOnMainThread(initTask =>
                        {
                            Debug.Log("task compl");
                            if (initTask.IsCompleted)
                            {
                                Debug.Log("User data initialized successfully.");
                                onInitialize?.Invoke();
                            }
                            else
                            {
                                Debug.LogError("Failed to initialize user data: " + initTask.Exception);
                            }
                        });
                    }
                    else
                    {
                        Debug.Log("User data already exists.");
                    }
                });
            }
            else
            {
                Debug.LogError("Failed to load configuration: " + configTask.Exception);
            }
        });
    }
}
