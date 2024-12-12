using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static Leaderboard instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(true);
        DisplayLeaderboard();
    }

    public void SetLeaderboardEntry (int newScore)
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighscore",
            FunctionParameter = new { score = newScore }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result => DisplayLeaderboard(),
            error => Debug.Log(error.ErrorMessage)
        );
    }

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result =>
            {
                UpdateLeaderboardUI(result.Leaderboard);
            },
            error =>
            {
                Debug.Log(error.ErrorMessage);
            });
    }

    void UpdateLeaderboardUI (List<PlayerLeaderboardEntry> leaderboard)
    {
        for (int i = 0; i < leaderboardEntries.Length; i++)
        {
            leaderboardEntries[i].SetActive(i < leaderboard.Count);

            if (i >= leaderboard.Count)
            {
                continue;
            }

            leaderboardEntries[i].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = (leaderboard[i].Position + 1) + ". " + leaderboard[i].DisplayName;
            leaderboardEntries[i].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = (-(float)leaderboard[i].StatValue * 0.001f).ToString("F2");
        }
    }
}
