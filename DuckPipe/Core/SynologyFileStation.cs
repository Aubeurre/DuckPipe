using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using DuckPipe;

public class SynologyFileStation
{
    private readonly SynologySession _session; 
    public SynologySession Session => _session;

    public SynologyFileStation(SynologySession session)
    {
        _session = session;
    }

    public async Task<List<string>> ListFoldersAsync(string folderPath)
    {
        var encodedPath = HttpUtility.UrlEncode(folderPath);
        var url = $"/webapi/entry.cgi?api=SYNO.FileStation.List&version=2&method=list&folder_path={encodedPath}&additional=real_path";

        string json = await _session.GetAsync(url);

        // Parsing JSON au lieu de Regex
        var folderNames = new List<string>();

        using (var doc = JsonDocument.Parse(json))
        {
            var items = doc.RootElement.GetProperty("data").GetProperty("files");

            foreach (var item in items.EnumerateArray())
            {
                if (item.TryGetProperty("isdir", out var isDir) && isDir.GetBoolean())
                {
                    folderNames.Add(item.GetProperty("name").GetString());
                }
            }
        }

        return folderNames;
    }
    public async Task CreateFolderAsync(string folderPath)
    {
        string parent = Path.GetDirectoryName(folderPath)?.Replace("\\", "/") ?? "/";
        string name = Path.GetFileName(folderPath);

        var url = "/webapi/entry.cgi?api=SYNO.FileStation.CreateFolder" +
                  "&version=2&method=create" +
                  $"&folder_path={HttpUtility.UrlEncode(parent)}" +
                  $"&name={HttpUtility.UrlEncode(name)}";

        string json = await _session.GetAsync(url);

        if (!Regex.IsMatch(json, "\"success\"\\s*:\\s*true"))
            throw new Exception($"Échec de la création du dossier sur le NAS : {folderPath}");
    }


    public async Task CreateFileAsync(string filePath, byte[] content)
    {

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["api"] = "SYNO.FileStation.Upload";
        query["version"] = "2";
        query["method"] = "upload";
        query["overwrite"] = "true";
        query["path"] = Path.GetDirectoryName(filePath)?.Replace("\\", "/") ?? "/";

        var fullUrl = $"{_session.BaseUrl}/webapi/entry.cgi?{query}";

        var fileName = Path.GetFileName(filePath);

        using var contentStream = new MemoryStream(content);
        using var fileContent = new StreamContent(contentStream);
        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        {
            Name = "\"file\"",
            FileName = $"\"{fileName}\""
        };
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        using var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent, "file", fileName);

        var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
        {
            Content = multipartContent
        };

        // Ajout manuel du cookie sid
        request.Headers.Add("Cookie", $"id={_session.Sid}");

        // Si besoin, aussi :
        // request.Headers.Add("X-SYNO-TOKEN", _session.Sid);

        var response = await _session.HttpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode || !json.Contains("\"success\":true"))
            throw new Exception($"Échec de la création du fichier '{filePath}' sur le NAS : {json}");
    }





}
