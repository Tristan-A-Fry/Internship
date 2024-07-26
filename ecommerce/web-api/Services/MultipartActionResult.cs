using ecommapp.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class MultipartActionResult : ActionResult
{
    private readonly OrderResponseDto _orderResponse;
    private readonly byte[] _fileContents;
    private readonly string _fileName;

    public MultipartActionResult(OrderResponseDto orderResponse, byte[] fileContents, string fileName)
    {
        _orderResponse = orderResponse;
        _fileContents = fileContents;
        _fileName = fileName;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "multipart/mixed; boundary=--boundary";

        await using var writer = new StreamWriter(response.Body);
        await writer.WriteLineAsync("--boundary");
        await writer.WriteLineAsync($"Content-Type: application/json; charset=utf-8");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync(System.Text.Json.JsonSerializer.Serialize(_orderResponse));
        await writer.WriteLineAsync("--boundary");
        await writer.WriteLineAsync($"Content-Disposition: attachment; filename={_fileName}");
        await writer.WriteLineAsync("Content-Type: text/plain");
        await writer.WriteLineAsync();
        await writer.FlushAsync();
        await response.Body.WriteAsync(_fileContents, 0, _fileContents.Length);
        await writer.WriteLineAsync();
        await writer.WriteLineAsync("--boundary--");
        await writer.FlushAsync();
    }
}
