namespace RuralAssets.WebApplication
{
    public class ModuleConfigOptions
    {
        public bool EnableAuthorization { get; set; }
        public bool EnableIdCardCheck { get; set; }
        public string CryptoKey { get; set; }
        public bool EnableCrypto { get; set; }
    }
}