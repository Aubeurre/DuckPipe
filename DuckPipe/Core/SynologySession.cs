using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

public class SynologySession : IDisposable
{
    public string Sid { get; private set; }
    private readonly HttpClient _client;
    private readonly CookieContainer _cookieContainer;
    private readonly string _baseUrl;
    public HttpClient HttpClient => _client;
    public string BaseUrl => _baseUrl;

    private SynologySession(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            ServerCertificateCustomValidationCallback = (s, cert, chain, sslPolicyErrors) => true
        };
        _client = new HttpClient(handler);
    }

    public static async Task<SynologySession> CreateAsync(string baseUrl, string username, string password)
    {
        var session = new SynologySession(baseUrl);
        await session.AuthenticateAsync(username, password);
        return session;
    }

    private async Task AuthenticateAsync(string username, string password)
    {
        var loginUrl = $"{_baseUrl}/webapi/auth.cgi?api=SYNO.API.Auth&version=6&method=login" +
                       $"&account={HttpUtility.UrlEncode(username)}&passwd={HttpUtility.UrlEncode(password)}" +
                       $"&session=FileStation&format=sid";

        var response = await _client.GetStringAsync(loginUrl);

        var match = Regex.Match(response, "\"sid\"\\s*:\\s*\"(.*?)\"");
        if (!match.Success)
            throw new Exception("Échec de l'authentification Synology API.");

        Sid = match.Groups[1].Value;

        // Ajout du cookie pour la session avec le nom "id" (vérifier dans ton cas précis)
        _cookieContainer.Add(new Uri(_baseUrl), new Cookie("id", Sid));
    }

    public async Task<string> GetAsync(string url)
    {
        return await _client.GetStringAsync($"{_baseUrl}{url}&_sid={Sid}");
    }

    public void Dispose()
    {
        _client?.Dispose();
    }

    public async Task LogoutAsync()
    {
        await _client.GetAsync($"{_baseUrl}/webapi/auth.cgi?api=SYNO.API.Auth&version=6&method=logout&session=FileStation&_sid={Sid}");
    }
}
