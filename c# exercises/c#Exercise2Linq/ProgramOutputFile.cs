using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class ProgramOutputFile
{
    public static void OutputFile()
    {
        var products = ProductData.GetProducts();
        var outputPath = "output.txt"; // Specify the output file path

        // Determine column widths based on data
        var columnWidths = GetColumnWidths(products);

        // Create or overwrite the output file
        using (var writer = new StreamWriter(outputPath))
        {
            // Write header
            WriteHeader(writer, columnWidths);

            // Write products
            WriteProducts(writer, products, columnWidths);

            Console.WriteLine($"Output file generated: {Path.GetFullPath(outputPath)}");
        }
    }

    private static Dictionary<string, int> GetColumnWidths(List<Product> products)
    {
        // Initialize column widths with minimum values
        var columnWidths = new Dictionary<string, int>
        {
            { "Name", "Name".Length },
            { "Category", "Category".Length },
            { "Price", "Price".Length }
        };

        // Update column widths based on data
        foreach (var product in products)
        {
            if (product.Name.Length > columnWidths["Name"])
                columnWidths["Name"] = product.Name.Length;

            if (product.Category.Length > columnWidths["Category"])
                columnWidths["Category"] = product.Category.Length;

            // Format price to ensure correct length calculation
            string formattedPrice = $"{product.Price:C}";
            if (formattedPrice.Length > columnWidths["Price"])
                columnWidths["Price"] = formattedPrice.Length;
        }

        // Add padding for aesthetics
        foreach (var key in columnWidths.Keys.ToList())
        {
            columnWidths[key] += 2; // Add padding of 2 spaces
        }

        return columnWidths;
    }

    private static void WriteHeader(StreamWriter writer, Dictionary<string, int> columnWidths)
    {
        // Calculate total width based on column widths
        int totalWidth = columnWidths.Sum(kv => kv.Value) + 44; // Adjusted for fixed length of characters

        // Write header lines
        writer.WriteLine(new string('-', totalWidth));
        writer.WriteLine($"-----     Name     -----     Category     -----     Price     ------");
        writer.WriteLine(new string('-', totalWidth));
    }


    private static void WriteProducts(StreamWriter writer, List<Product> products, Dictionary<string, int> columnWidths)
    {
        // Ensure columnWidths keys are constant expressions
        int nameWidth = columnWidths["Name"];
        int categoryWidth = columnWidths["Category"];
        int priceWidth = columnWidths["Price"];

        foreach (var product in products)
        {
            string formattedName = product.Name.PadRight(nameWidth);
            string formattedCategory = product.Category.PadRight(categoryWidth);
            string formattedPrice = $"{product.Price:C}".PadLeft(priceWidth);

            writer.WriteLine($"{formattedName} {formattedCategory} {formattedPrice}");
        }
    }



}



