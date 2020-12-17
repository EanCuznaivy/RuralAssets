namespace RuralAssets.WebApplication
{
    public class ConfigOptions
    {
        public string RuralAssetsConnectString { get; set; }
        public string CreditUri { get; set; }
        public string BlockChainEndpoint { get; set; }
        public string AccountAddress { get; set; }
        public string AccountPassword { get; set; }
        public string RuralContractAddress { get; set; }
        
        public string FileSaveDir { get; set; }
    }
}