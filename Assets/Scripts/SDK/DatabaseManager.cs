using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Firebase.Auth;
using Assets.Scripts.Main;
using Zenject;

public class DatabaseManager
{
    private string _userId => FirebaseAuth.DefaultInstance.CurrentUser.UserId;

    private LevelManager _levelManager;

    public void LoadLevelData(Action<Dictionary<int,LevelData>> onLoaded = null)
    {
        var configRef = FirebaseDatabase.DefaultInstance.RootReference.Child("config").Child("totalLevels")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    int totalLevels = int.Parse(task.Result.Value.ToString());
                    var userLevelsRef = FirebaseDatabase.DefaultInstance
                        .RootReference.Child("users").Child(_userId).Child("levels");
                    userLevelsRef.GetValueAsync().ContinueWithOnMainThread(levelsTask =>
                    {
                        if (levelsTask.IsCompleted && !levelsTask.IsFaulted)
                        {
                            var dataSnapshot = levelsTask.Result;
                            var levels = new Dictionary<int, LevelData>();
                            var updates = new Dictionary<string, object>();

                            for (int levelNum = 1; levelNum <= totalLevels; levelNum++)
                            {
                                if (dataSnapshot.HasChild(levelNum.ToString()))
                                {
                                    var json = dataSnapshot.Child(levelNum.ToString()).GetRawJsonValue();
                                    LevelData levelData = JsonConvert.DeserializeObject<LevelData>(json);
                                    levels.Add(levelNum, levelData);
                                }
                                else
                                {
                                    Debug.Log($"Level {levelNum} not found. Added data to update.");

                                    var defaultData = new LevelData();
                                    if (levelNum == 1 || levels[levelNum - 1].IsCompleted)
                                        defaultData.IsUnlocked = true;

                                    updates.Add(levelNum.ToString(), defaultData);
                                    levels.Add(levelNum, defaultData);
                                }
                            }

                            if (updates.Count > 0)
                            {
                                Debug.Log($"New levels data has been found. Updating {updates.Count} levels...");
                                var levelsData = updates.ToDictionary(
                                        kvp => kvp.Key.ToString(),
                                        kvp => kvp.Value
                                    );
                                var json = JsonConvert.SerializeObject(levelsData);
                                userLevelsRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(updateTask =>
                                {
                                    if (updateTask.IsCompleted)
                                    {
                                        Debug.Log("New levels added for the user.");
                                    }
                                    else
                                    {
                                        Debug.LogError("Failed to add new levels: " + updateTask.Exception);
                                    }
                                });
                            }
                            onLoaded.Invoke(levels);
                        }
                        else
                        {
                            Debug.LogError($"Can't get user levels data! {task.Exception}");
                            onLoaded.Invoke(new Dictionary<int, LevelData>());
                        }
                    });
                }
                else
                {
                    Debug.LogError($"Can't get config ref:{task.Exception}");
                    onLoaded.Invoke(new Dictionary<int, LevelData>());
                }
            });
    }

    public void OnLevelComplete(int levelNum, PlayerStats stats)
    {
        var currentLevelData = _levelManager.LevelsData[levelNum];
        if (currentLevelData.MaxTime == 0 || stats.TimeSpent < currentLevelData.MaxTime)
            currentLevelData.MaxTime = stats.TimeSpent;
        if (stats.Coins > currentLevelData.MaxScore)
            currentLevelData.MaxScore = stats.Coins;
        currentLevelData.IsCompleted = true;

        var nextLevelNum = levelNum + 1;
        if (_levelManager.LevelsData.TryGetValue(nextLevelNum, out var data))
            data.IsUnlocked = true;

        if (data != null)
        {
            var lockUpdate = new Dictionary<string, object>
            {
                { "IsUnlocked", true }
            };
            FirebaseDatabase.DefaultInstance.RootReference
                .Child("users").Child(_userId).Child("levels").Child(nextLevelNum.ToString())
                .UpdateChildrenAsync(lockUpdate)
                .ContinueWithOnMainThread(result =>
                {
                    if (result.IsCompleted)
                    {
                        Debug.Log("Level data updated!");
                    }
                    else
                    {
                        Debug.LogError("Can't update level data!");
                    }
                });
        }

        var updatedCurrentData = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(currentLevelData));

        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users").Child(_userId).Child("levels").Child(levelNum.ToString())
            .UpdateChildrenAsync(updatedCurrentData)
            .ContinueWithOnMainThread(result =>
            {
                if (result.IsCompleted)
                {
                    Debug.Log("Level data updated!");
                }
                else
                {
                    Debug.LogError("Can't update level data!");
                }
            });
    }

    [Inject]
    public void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }
}
