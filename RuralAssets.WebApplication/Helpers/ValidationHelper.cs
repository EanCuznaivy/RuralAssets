using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace RuralAssets.WebApplication
{
    public interface IValidationService
    {
        bool ValidateIdCard(string idCard);
        bool ValidateYear(string year);
    }

    public class ValidationService : IValidationService
    {
        private readonly ModuleConfigOptions _moduleOptions;

        public ValidationService(IOptionsSnapshot<ModuleConfigOptions> moduleOptions)
        {
            _moduleOptions = moduleOptions.Value;
        }

        public bool ValidateIdCard(string idCard)
        {
            if (_moduleOptions.EnableIdCardCheck)
            {
                return new Regex(
                        @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$")
                    .Match(idCard).Success;
            }

            return idCard.Length == 18;
        }

        public bool ValidateYear(string year)
        {
            return new Regex(@"^(19|20)\d{2}$").Match(year).Success;
        }
    }
}