using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using SimpleJSON;
using System.Linq;

public class FirebaseRankingManager : MonoBehaviour
{
    public const string url = "https://ratopiadata-default-rtdb.asia-southeast1.firebasedatabase.app/";

    public const string secret = "06jbWCcOcVQZo16HkfPkBNV2s2ZSKhpbQtuKIpWV";
    

    [System.Serializable]
    public class Ranking
    {
        public List<Player> PlayersData = new List<Player>();
    }

    public Ranking ranking;

    public void CalRankScore()
    {
        GetPlayerData();
        
        List<Player> sortingPlayer = new List<Player>();

        sortingPlayer = ranking.PlayersData.OrderByDescending(player => player.PlayerScore).ToList();
        ranking.PlayersData = sortingPlayer;
        
        PushPlayerData();
    }

    private void Start()
    {
        PushPlayerData();
        CalRankScore();
    }

    #region test
    public class TestData
    {
        public int num = 75;
        public string name = "fucker";
    }

    public TestData testData = new TestData();
    

    #endregion
    

    public void PushPlayerData()
    {
        string urlData = $"{url}/PlayerData.json?auth={secret}";
        RestClient.Put<Ranking>(urlData, ranking).Then(testData =>
        {
            //Debug.Log("Upload Data Complete");
        }).Catch(error =>
        {
            Debug.Log("error on server?");
        });

    }

    private int testnum = 0;
    public void GetPlayerData()
    {
        string urlData = $"{url}/PlayerData.json?auth={secret}";

        RestClient.Get(urlData).Then(response =>
        {
            //Debug.Log(response.Text);

            #region test

            JSONNode jsonNode = JSONNode.Parse(response.Text);
            testnum = jsonNode["num"];
            //Debug.Log(testnum);

            #endregion

        }).Catch(error =>
        {
            Debug.Log("Error can't get data");
        });
    }
}
