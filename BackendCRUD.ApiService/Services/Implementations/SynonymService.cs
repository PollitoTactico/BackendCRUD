using BackendCRUD.ApiService.Services.Interfaces;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class SynonymService : ISynonymService
    {
        private readonly Dictionary<string, List<string>> _synonyms = new()
        {
            { "c#", new List<string> { "csharp", "c-sharp" } },
            { "azure", new List<string> { "microsoft azure", "azure cloud" } },
            { "sql", new List<string> { "database", "bases de datos" } },
            { "javascript", new List<string> { "js", "ecmascript" } },
            { "react", new List<string> { "reactjs", "react.js" } },
            { "angular", new List<string> { "angularjs", "angular.js" } },
            { "net", new List<string> { ".net", "dotnet" } },
            { "python", new List<string> { "py" } },
            { "java", new List<string> { "jdk" } }
        };

        public List<string> GetSynonyms(string word)
        {
            return _synonyms.TryGetValue(word, out var synonyms) ? synonyms : new List<string>();
        }
    }
}