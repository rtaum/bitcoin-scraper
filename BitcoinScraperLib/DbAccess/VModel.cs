namespace BitcoinScraperLib.DbAccess
{
    public abstract class VModel
    {
        public int TransactionId { get; set; }
        public string Address { get; set; }
    }
}
