using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DatabaseManager
{
    public static void LoadLevelData(string userId, Action<Dictionary<int,LevelData>> onLoaded = null)
    {
        var _database = FirebaseDatabase.DefaultInstance.RootReference;
        _database.Child("users").Child(userId).Child("levels")
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        var levelDict = snapshot.Value as Dictionary<string,object>;
                        var levels = levelDict.ToDictionary(
                            kvp => int.Parse(kvp.Key),
                            kvp => JsonUtility.FromJson<LevelData>(kvp.Value.ToString())
                            );
                        onLoaded(levels);
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
        configRef.GetValueAsync().ContinueWith(configTask =>
        {
            if (configTask.IsCompleted)
            {
                DataSnapshot configSnapshot = configTask.Result;
                int totalLevels = int.Parse(configSnapshot.Child("totalLevels").Value.ToString());
                DatabaseReference userRef = FirebaseDatabase.DefaultInstance
                   .RootReference.Child("users").Child(userId);

                userRef.GetValueAsync().ContinueWith(userTask => 
                {
                    DataSnapshot userSnapshot = userTask.Result;
                    if (!userSnapshot.Exists)
                    {
                        var levels = new Dictionary<int, LevelData>();
                        for (int i = 0; i < totalLevels; i++)
                        {
                            var levelData = new LevelData(0, 0, false);
                            levels.Add(i, levelData);
                        }

                        levels[0].IsUnlocked = true;

                        var initialData = new
                        {
                            levelsData = levels
                        };
                        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        userRef.SetValueAsync(initialData).ContinueWith(initTask =>
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
