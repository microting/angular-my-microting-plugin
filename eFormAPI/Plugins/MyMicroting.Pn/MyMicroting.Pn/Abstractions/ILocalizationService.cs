namespace MyMicroting.Pn.Abstractions
{
    public interface ILocalizationService
    {
        string GetString(string key);
        string GetString(string format, params object[] args);
    }
}
