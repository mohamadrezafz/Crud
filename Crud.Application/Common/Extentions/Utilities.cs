

using IbanNet;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace Crud.Application.Common.Extentions;

public static class Utilities
{
    public static bool IsPhoneNumberValid(this string phoneNumber)
    {
        // Use Google's LibPhoneNumber to validate mobile numbers
        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var parsedNumber = phoneUtil.Parse(phoneNumber, "");
            return phoneUtil.IsValidNumber(parsedNumber);
        }
        catch (Exception)
        {

            return false;
        }

    }

    public static bool IsValidIban(string iban)
    {

        IIbanValidator validator = new IbanValidator();
        ValidationResult validationResult = validator.Validate(iban);
        if (validationResult.IsValid)
            return true;
        return false;
    }


}
