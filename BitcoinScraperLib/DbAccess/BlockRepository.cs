using System;
using System.Collections.Generic;
using System.Text;

namespace BitcoinScraperLib.DbAccess
{
    public class BlockRepository : IRepository
    {
        public Block Get(int index)
        {
            throw new NotImplementedException();
        }

        public bool TrySave(Block block)
        {
            throw new NotImplementedException();
        }

        //public bool TrySave(Block block)
        //{
        //    var storedBlock = Get(block.Index);
        //    if (storedBlock != null)
        //        return false;



        //}
    }
}
