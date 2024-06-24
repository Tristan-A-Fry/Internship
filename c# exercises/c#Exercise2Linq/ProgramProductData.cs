using System;
using System.Linq;

public class ProgramProductData 
{
    public static void ProcessProductData()
    {
        //gets the list og products
        var products = ProductData.GetProducts();

        var totalCostPerCategory = products
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                TotalCost = g.Sum(p => p.Price)
            });

        Console.WriteLine("Total cost per category:");
        foreach (var category in totalCostPerCategory)
        {
            Console.WriteLine($"{category.Category}: {category.TotalCost:C}m");
        }

        // 2. Find the product with the highest price per category
        var highestPricePerCategory = products
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                HighestPricedProduct = g.OrderByDescending(p => p.Price).First()
            });

        Console.WriteLine("\nProduct with the highest price per category:");
        foreach (var category in highestPricePerCategory)
        {
            Console.WriteLine($"{category.Category}: {category.HighestPricedProduct.Name} ({category.HighestPricedProduct.Price:C}m)");
        }

        // 3. Sort the categories based on the sum of the product's total cost
        var sortedCategoriesByTotalCost = totalCostPerCategory
            .OrderByDescending(c => c.TotalCost);

        Console.WriteLine("\nCategories sorted by total cost:");
        foreach (var category in sortedCategoriesByTotalCost)
        {
            Console.WriteLine($"{category.Category}: {category.TotalCost:C}m");
        }
    }
}
