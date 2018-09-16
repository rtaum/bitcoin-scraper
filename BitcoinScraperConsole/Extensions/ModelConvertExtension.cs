using BitcoinScraperDatabase.Models;
using BitcoinScraperLib;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinScraperConsole.Extensions
{
    internal static class ModelConvertExtension
    {
        public static BlockModel BuildDomainModel(Block block)
        {
            var blockModel = new BlockModel()
            {
                Hash = block.Hash,
                Index = block.Index
            };

            blockModel.Transactions = block.Transactions.
                Where(t => t != null && t.Hash != null).
                Select(t => new TransactionModel()
                {
                    Hash = t.Hash,
                    VIns = GetVinModel(t),
                    VOuts = GetVoutModel(t)
                }).ToList();
            return blockModel;
        }

        private static IList<VOutModel> GetVoutModel(Transaction t)
        {
            return t.VOuts.SelectMany(vout => vout.ScriptPubKey?.Addresses ?? new string[0],
                (v, a) => new VOutModel()
                {
                    Address = a,
                    TransactionHash = v.TransactionHash,
                    Value = v.Value
                }).ToList();
        }

        private static List<VInModel> GetVinModel(Transaction t)
        {
            return t.VIns.Select(vin => new VInModel()
            {
                Address = vin.Address,
                TransactionHash = vin.TransactionHash,
                Value = vin.Value
            }).ToList();
        }
    }
}
