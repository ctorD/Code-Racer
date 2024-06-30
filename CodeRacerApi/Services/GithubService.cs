using System.Text.RegularExpressions;
using Octokit;

namespace CodeRacerApi.Services;

public class GithubService
{
    private readonly GitHubClient _client;

    public GithubService(GitHubClient client)
    {
        _client = client;
    }
    
    public async Task<string> GetSnippet(Language lang)
    {
        var codeSampleResponse = await _client.Search.SearchCode(new SearchCodeRequest(GetKeyword(lang))
        {
            Language = lang,
        });
        var codeSample = codeSampleResponse.Items[3];

        var testCont = await _client.Repository.Content
            .GetAllContents(repositoryId: codeSample.Repository.Id, codeSample.Path);
        var rawCodeString = testCont[0].Content;
        //Trim out function block
        rawCodeString = rawCodeString.Trim();
        var keyword = GetKeyword(lang);
        var allIndexes = GetAllIndexes(rawCodeString, keyword);
        var randomIndex = new Random().Next(0, allIndexes.Count);
        rawCodeString =
            rawCodeString.Substring(allIndexes[randomIndex], rawCodeString.Length - allIndexes[randomIndex]);
        var beforeFunctionBlock = rawCodeString.Substring(0, rawCodeString.IndexOf('{'));
        var matches = Regex.Matches(rawCodeString, @"{((?>[^{}]+|{(?<c>)|}(?<-c>))*(?(c)(?!)))");
        var functionBlock = matches[0].Value + "\n }";
        var fullFunction = beforeFunctionBlock + functionBlock;

        return fullFunction;
    }

    private string GetKeyword(Language lang)
    {
        switch (lang)
        {
            case Language.JavaScript:
            case Language.TypeScript:
                return "function";
            case Language.CSharp:
                return "public";
        }

        return "";
    }

    private List<int> GetAllIndexes(string source, string matchString)
    {
        var test = new List<int>();
        matchString = Regex.Escape(matchString);
        foreach (Match match in Regex.Matches(source, matchString)) test.Add(match.Index);

        return test;
    }

}