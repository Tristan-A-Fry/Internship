using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

public class ProgramSearchBOEM
{
    public static async Task Main(string[] args)
    {
        await searchBoem();
    }

    public static async Task searchBoem()
    {
        await DownloadAndExtractData();

        Console.WriteLine("Enter platform's area code:");
        string? areaCode = Console.ReadLine()?.Trim();

        Console.WriteLine("Enter platform's block number:");
        string? blockNumber = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(areaCode) && !string.IsNullOrEmpty(blockNumber))
        {
            SearchAndDisplayPlatformData(areaCode, blockNumber);
        }
        else
        {
            Console.WriteLine("Invalid input. Both area code and block number are required.");
        }
    }

    private static async Task DownloadAndExtractData()
    {
        string zipFileUrl = "https://www.data.boem.gov/Platform/Files/PlatStrucRawData.zip";
        string tempZipFile = Path.GetTempFileName();
        string extractPath = Path.Combine(Path.GetTempPath(), "BOEMData");

        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(zipFileUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                    {
                        using (Stream streamToWriteTo = File.Open(tempZipFile, FileMode.Create))
                        {
                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        }
                    }
                }
            }

            // Delete the existing extraction directory if it exists
            if (Directory.Exists(extractPath))
            {
                Directory.Delete(extractPath, true);
            }

            ZipFile.ExtractToDirectory(tempZipFile, extractPath);

            Console.WriteLine($"Data extracted to: {extractPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading or extracting data: {ex.Message}");
        }
        finally
        {
            File.Delete(tempZipFile); // Clean up temporary zip file
        }
    }

    private static void SearchAndDisplayPlatformData(string areaCode, string blockNumber)
    {
        string extractPath = Path.Combine(Path.GetTempPath(), "BOEMData", "PlatStrucRawData");
        string dataFilePath = Path.Combine(extractPath, "mv_platstruc_structures.txt");

        if (!File.Exists(dataFilePath))
        {
            Console.WriteLine("Data file not found. Please ensure the data was downloaded correctly.");
            return;
        }

        try
        {
            // Read all lines from the data file
            string[] lines = File.ReadAllLines(dataFilePath, System.Text.Encoding.UTF8);

            // Header line contains field names, split it to get column indices
            string[] headers = lines[0].Split(',');
            var columnIndex = new System.Collections.Generic.Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                columnIndex[headers[i].Trim('"')] = i;
            }

            // Find matching platform
            bool found = false;
            for (int i = 1; i < lines.Length; i++) // Start from 1 to skip header
            {
                string[] fields = lines[i].Split(',');

                string platformArea = fields[columnIndex["AREA_CODE"]].Trim('"');
                string platformBlock = fields[columnIndex["BLOCK_NUMBER"]].Trim('"').Trim();
                string platformField = fields[columnIndex["FIELD_NAME_CODE"]].Trim('"');
                string platformStructure = fields[columnIndex["STRUCTURE_NAME"]].Trim('"');
                string platformBuscAscName = fields[columnIndex["BUS_ASC_NAME"]].Trim('"');

                if (platformArea == areaCode && platformBlock == blockNumber)
                {
                    Console.WriteLine($"Platform found:");
                    Console.WriteLine($"Area Code: {platformArea}");
                    Console.WriteLine($"Block Number: {platformBlock}");
                    Console.WriteLine($"Field: {platformField}");
                    Console.WriteLine($"Structure: {platformStructure}");
                    Console.WriteLine($"Busc Asc Name: {platformBuscAscName}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No platform found matching the specified area code and block number.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing data: {ex.Message}");
        }
    }

}
