namespace BitcoinScraperLib.DbAccess
{
    public interface IRepository
    {
        bool TrySave(Block block);

        Block Get(int index);
    }
}
