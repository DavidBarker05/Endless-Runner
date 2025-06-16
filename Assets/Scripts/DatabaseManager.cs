using Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    DatabaseReference databaseReference;
    TaskCompletionSource<bool> databaseInitialised = new TaskCompletionSource<bool>();

    async void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        await InitialiseFirebase();
        databaseInitialised.TrySetResult(true);
    }

    async Task InitialiseFirebase()
    {
        DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available) databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task WaitUntilInitialised() => await databaseInitialised.Task;

    public async Task SaveScore(string username, int score)
    {
        await WaitUntilInitialised();
        if (databaseReference == null) return;
        try
        {
            DataSnapshot dataSnapshot = await databaseReference.Child("Players").Child(username).GetValueAsync();
            if (dataSnapshot.Exists)
            {
                int.TryParse(dataSnapshot.Child("Score")?.Value?.ToString(), out int highScore);
                if (score < highScore) return;
            }
            Dictionary<string, object> data = new Dictionary<string, object> { { "Score", score } };
            await databaseReference.Child("Players").Child(username).SetValueAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save to database. REASON: {e}");
        }
    }

    public async Task<Dictionary<string, int>> LoadLeaderboard()
    {
        Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        if (databaseReference == null) return leaderboard;
        try
        {
            DataSnapshot dataSnapshot = await databaseReference.Child("Players").OrderByChild("Score").LimitToLast(10).GetValueAsync();
            if (!dataSnapshot.Exists) return leaderboard;
            foreach (DataSnapshot child in dataSnapshot.Children.Reverse())
            {
                string username = child?.Key ?? "";
                int.TryParse(child?.Child("Score")?.Value?.ToString(), out int score);
                leaderboard.Add(username, score);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load from database. REASON: {e}");
        }
        return leaderboard;
    }
}
