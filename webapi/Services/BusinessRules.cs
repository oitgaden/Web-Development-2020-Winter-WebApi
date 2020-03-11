public static class BusinessRules
{
    public static bool isLowInventory(Product product) => 
        product.Count < lowInventoryCount;

    public static bool isOnSale(Product product) =>
        product.Name.ToLower().StartsWith(onSaleKey);

    public static int lowInventoryCount = 3;

    public static string onSaleKey = "b";
}