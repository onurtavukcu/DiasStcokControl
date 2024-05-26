using DiasShared.DTOs.InputDTO;

namespace DiasDomain.ProductCommands.ValidationOperations
{
    public static class ProductValidationCommand
    {
        public static bool Validate(AddProductDTO product)
        {
            if (ProductNameValidator(product.Name) || IntTypeValidator(product.StockQuantity) || DoubleTypeValidator(product.Price))
            {
                return false;
            }
            return true;
        }

        public static bool ProductNameValidator(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return false;

            foreach (char c in productName)
            {
                if (char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IntTypeValidator(int number)
        {
            if (int.IsNegative(number) || number == 0)
            {
                return false;
            }

            return true;
        }

        public static bool DoubleTypeValidator(decimal number)
        {
            if (decimal.IsNegative(number) || number == 0)
            {
                return false;
            }

            return true;
        }
    }
}
