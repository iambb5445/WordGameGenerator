using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public abstract class API
{
    public abstract List<string> getCandidateWords(string theme, int count);
}

public abstract class NetworkAPI: API
{
    protected string baseUrl;
    protected NetworkAPI(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }
}

public class CustomWordsAPI: API
{
    private List<string> words;
    public void setWords(List<string> words)
    {
        this.words = words;
    }
    public override List<string> getCandidateWords(string theme, int count)
    {
        return words;
    }
}

public abstract class APIwithReturnType<T>: NetworkAPI
{
    public APIwithReturnType(string baseUrl): base(baseUrl)
    {
    }
    protected T get(params string[] args)
    {
        string url = string.Format(baseUrl, args);
        ServicePointManager.ServerCertificateValidationCallback += delegate {return true;}; 
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        T unwrappedResponse = JsonUtility.FromJson<T>(alterResponseJson(json));
        return unwrappedResponse;
    }
    protected abstract string alterResponseJson(string json);
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
    [System.Serializable]
    public class DatamuseResponse<T>
    {
        public T[] words;
    }
    public abstract class DatamuseAPI<T>: APIwithReturnType<DatamuseResponse<T>>
    {
        public DatamuseAPI(string baseUrl): base(baseUrl)
        {
        }
        protected override string alterResponseJson(string json)
        {
            return "{\"words\":" + json + "}";
        }
    }
    public class AssociationAPI: DatamuseAPI<AssociationAPI.DatamuseWord>
    {
        [System.Serializable]
        public class DatamuseWord
        {
            public string word;
            public int score;
        }
        public AssociationAPI(): base("https://api.datamuse.com/words?rel_trg={0}")
        {
        }
        public override List<string> getCandidateWords(string theme, int count)
        {
            DatamuseResponse<DatamuseWord> response = this.get(theme);
            Dictionary<string, int> wordScores = new Dictionary<string, int>();
            foreach (DatamuseWord word in response.words)
            {
                wordScores[word.word] = word.score;
            }
            return sampleWords(count, false, wordScores);
        }
    }
    public class SimilarMeaningAPI: DatamuseAPI<SimilarMeaningAPI.DatamuseWord>
    {
        [System.Serializable]
        public class DatamuseWord
        {
            public string word;
            public int score;
            public string[] tags;
        }
        public SimilarMeaningAPI(): base("https://api.datamuse.com/words?ml={0}")
        {
        }
        public override List<string> getCandidateWords(string theme, int count)
        {
            DatamuseResponse<DatamuseWord> response = this.get(theme);
            Dictionary<string, int> wordScores = new Dictionary<string, int>();
            foreach (DatamuseWord word in response.words)
            {
                wordScores[word.word] = word.score;
            }
            return sampleWords(count, false, wordScores);
        }
    }
    public class RhymeAPI: DatamuseAPI<RhymeAPI.DatamuseWord>
    {
        [System.Serializable]
        public class DatamuseWord
        {
            public string word;
            public int score;
            public int numSyllables;
        }
        public RhymeAPI(): base("https://api.datamuse.com/words?rel_rhy={0}")
        {
        }
        public override List<string> getCandidateWords(string theme, int count)
        {
            DatamuseResponse<DatamuseWord> response = this.get(theme);
            Dictionary<string, int> wordScores = new Dictionary<string, int>();
            foreach (DatamuseWord word in response.words)
            {
                wordScores[word.word] = word.score;
            }
            return sampleWords(count, false, wordScores);
        }
    }
}