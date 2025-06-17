using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadLeaderboard : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> playerLabels = new List<TextMeshProUGUI>();

    void Start()
    { 
    }

    void Update()
    { 
    }

    async void OnEnable()
    {
        Dictionary<string, int> leaderBoard = await DatabaseManager.Instance.LoadLeaderboard();
        int index = 0;
        foreach (KeyValuePair<string, int> kVP in leaderBoard)
        {
            if (index >= playerLabels.Count) break;
            playerLabels[index].text = $"{index + 1} - {kVP.Key} - {kVP.Value}";
            index++;
        }
    }
}
