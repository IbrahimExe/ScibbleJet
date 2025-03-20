using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using NUnit.Framework;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

public class UGSManager : MonoBehaviour
{
    public static UGSManager Instance;

    async void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;

            Debug.Log("Production Environment Loading...");

            var options = new InitializationOptions(); // Allows for storing usernames online.
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options); // Wait for a response from the server / Online service.

            Debug.Log("Production Envrionment Loaded!");

            await SignUpAnonymouslyAsync();

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    async Task SignUpAnonymouslyAsync() // Task blocks the execution of code until all the code in the Task is complete.
    {
        // Clearing session allwos for multiple scores to be submitted from the same computer (After a restart)
        // AuthenticationService.Instance.ClearSessionToken();

        // Create a guest profile for the player to quicly start playing:
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Signed in as Guest!");

        string randomName = "Guest" + Random.Range(1, 999);
        await AuthenticationService.Instance.UpdatePlayerNameAsync(randomName);
        Debug.Log($"Player Name: {AuthenticationService.Instance.PlayerName}");
    }

    public async void AddScore(string leaderboardId, float score)
    {
        Debug.Log("Adding score to UGS Leaderboard...");
        var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
        Debug.Log("Score Submitted!");
    }

    public async void GetScores(string leaderboardId)
    {
        Debug.Log("Loading Player Scores...");
        var scoreResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

        List<LeaderboardEntry> entries = scoreResponse.Results;

        foreach (var entry in entries)
        {
            Debug.Log($"Name: {entry.PlayerName} - {entry.Score}");
        }

        GameObject gm = GameObject.FindGameObjectWithTag("GameController");
        gm.GetComponent<GameManager>().ShowLeaderboardUI(entries);
    }

}
