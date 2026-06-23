using Microsoft.AspNetCore.Mvc;
using PrintUMKServer.Models;
using PrintUMKServer.Services.FileStorage;
using System.Security.Claims;
using PrintUMKServer.Services.Messaging;


public class PrintController : Controller
{
    private readonly IAzureBlobStorageService _blobStorageService;
    private readonly IAzureServiceBusService _serviceBusService;
    public PrintController(IAzureBlobStorageService blobStorageService, IAzureServiceBusService ServiceBusService)
    {
        _blobStorageService = blobStorageService;
        _serviceBusService = ServiceBusService;

    }

    // GET: /Print/Upload
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    // POST: /Print/Upload
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file, string printerName)
    {
        var jobId = Guid.NewGuid();

        string? userId = User.Identity?.IsAuthenticated == true
            ? User.FindFirstValue(ClaimTypes.NameIdentifier)
            : null;

        var blobPath = await _blobStorageService.UploadFileAsync(
            file, userId, jobId);

        var sasUrl = _blobStorageService.GenerateReadSasUrl(blobPath, TimeSpan.FromMinutes(30));
        Console.WriteLine(sasUrl);

        // TU TRZEBA PRZEKAZYWAC SAS DO TEGO BLOBU A NIE TYLKO SCIEZKE
        var message = new BusMessage
        {
            BlobUrl = sasUrl,
            PrinterName = printerName,
            UserId = userId,
            JobId = jobId.ToString()
        };

        await _serviceBusService.SendMessageAsync(printerName, message);

        // dalej: PrintJob, kolejka, płatność...
        // ⬅️ TU PÓŹNIEJ:
        // - zapis do Blob Storage
        // - wysłanie komunikatu do kolejki

        ViewBag.Message = "Plik został wysłany do druku";
        return View();
    }
}