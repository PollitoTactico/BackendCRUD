namespace BackendCRUD.ApiService.Services.Interfaces
{
    public interface ISynonymService
    {
        List<string> GetSynonyms(string word);
    }
}
