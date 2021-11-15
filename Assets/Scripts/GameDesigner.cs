using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;


public abstract class API<T>
{
    public string baseUrl;
    public API(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
    protected T get(params string[] args)
    {
        string url = string.Format(baseUrl, args);
        ServicePointManager.ServerCertificateValidationCallback += delegate {return true;}; 
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        T unwrappedResponse = JsonUtility.FromJson<T>("{\"words\":" + json + "}");
        return unwrappedResponse;
    }
    public abstract List<string> getCandidateWords(string theme, int count);
    protected List<string> sampleWords(int count, bool hasReplacement, Dictionary<string, int> wordScores)
    {
        List<string> samples = new List<string>();
        for(; count > 0 && wordScores.Count > 0; count--)
        {
            int totalScore = 0;
            foreach (KeyValuePair<string, int> keyValue in wordScores)
            {
                totalScore += keyValue.Value;
            }
            int chosenScore = Random.Range(0, totalScore);
            foreach (KeyValuePair<string, int> keyValue in wordScores)
            {
                if (chosenScore < keyValue.Value)
                {
                    samples.Add(keyValue.Key);
                    if (!hasReplacement)
                    {
                        wordScores.Remove(keyValue.Key);
                    }
                    break;
                }
                chosenScore -= keyValue.Value;
            }
        }
        return samples;
    }
}

namespace Datamuse
{
    public class AssociationAPI: API<AssociationAPI.DatamuseResponse>
    {
        [System.Serializable]
        public class DatamuseWord
        {
            public string word;
            public int score;
        }
        [System.Serializable]
        public class DatamuseResponse
        {
            public DatamuseWord[] words;
        }
        public AssociationAPI(): base("https://api.datamuse.com/words?rel_trg={0}")
        {
        }
        public override List<string> getCandidateWords(string theme, int count)
        {
            DatamuseResponse response = this.get(theme);
            Dictionary<string, int> wordScores = new Dictionary<string, int>();
            foreach (DatamuseWord word in response.words)
            {
                wordScores[word.word] = word.score;
            }
            return sampleWords(count, false, wordScores);
        }
    }
}

public class GameDesigner
{
    public void design(string theme, Level level)
    {
        Datamuse.AssociationAPI associationAPI = new Datamuse.AssociationAPI();
        // TODO choose candidates and their count based on difficulty
        List<string> candidateWords = associationAPI.getCandidateWords(theme, 3);
        level.initiate(candidateWords);
    }
}

public class WordGraph
{
    List<string> nodes;
    Dictionary<int, List<int>> edges;
    public WordGraph(List<string> words)
    {
        setNodes(words);
    }
    private List<Dictionary<string, List<int>>> getPartsPerWord(List<string> words)
    {
        List<Dictionary<string, List<int>>> partsPerWord = new List<Dictionary<string, List<int>>>();
        foreach (string word in words)
        {
            Dictionary<string, List<int>> parts = new Dictionary<string, List<int>>();
            int positionCounter = 0;
            foreach(char character in word)
            {
                string part = character.ToString();
                if (!parts.ContainsKey(part))
                {
                    parts[part] = new List<int>();
                }
                parts[part].Add(positionCounter);
                positionCounter++;
            }
            partsPerWord.Add(parts);
        }
        return partsPerWord;
    }
    private void setNodes(List<string> wrods)
    {
    }
}
