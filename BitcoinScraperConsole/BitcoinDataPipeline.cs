using BitcoinScraperConsole.Extensions;
using BitcoinScraperDatabase;
using BitcoinScraperDatabase.Models;
using BitcoinScraperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BitcoinScraperConsole
{
    public class BitcoinDataPipeline
    {
        /// <summary>
        /// Gets desired blocks count and pulls out indexes from the bitcoin site
        /// </summary>
        private readonly TransformManyBlock<int, int> _getBlocksIndexesBlock;

        /// <summary>
        /// Checks if a block with the given index already was processed
        /// </summary>
        private readonly TransformBlock<int, (int index, bool isProcessed)> _checkIfBlockIsProcessedBlock;

        /// <summary>
        /// Loads block hash code by index
        /// </summary>
        private readonly TransformBlock<(int index, bool isProcessed),
            (int Index, string BlockHash)> _loadBlockHashBlock;


        /// <summary>
        /// Loads transaction lists of the given block
        /// </summary>
        private readonly TransformBlock<(int Index, string BlockHash),
            (int Index, string BlockHash, IEnumerable<string> TransactionHashes)> _loadTransactionHashesBlock;

        /// <summary>
        /// Loads transaction details of the given transactions and build the block model
        /// </summary>
        private readonly TransformBlock<(int Index, string BlockHash, IEnumerable<string> TransactionHashes),
            Block> _loadTransactionsBlock;

        /// <summary>
        /// Save the block to the database
        /// </summary>
        private readonly ActionBlock<Block> _saveInDatabaseBlock;

        /// <summary>
        /// Logs on console that the given block is already processed
        /// </summary>
        private readonly ActionBlock<(int index, bool isProcessed)> _logDoubleProcessingBlock;

        public BitcoinDataPipeline()
        {
            ExecutionDataflowBlockOptions options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4
            };

            _getBlocksIndexesBlock = new TransformManyBlock<int, int>(async i => await GetLastIndexes(i), options);
            _checkIfBlockIsProcessedBlock = new TransformBlock<int, (int index, bool isProcessed)>(async i => await CheckIfBlockIsProcessedBlock(i), options);
            _loadBlockHashBlock = new TransformBlock<(int index, bool isProcessed), (int Index, string BlockHash)>(i => GetBlockHashByIndex(i.index), options);
            _loadTransactionHashesBlock = new TransformBlock<(int Index, string BlockHash),
                (int Index, string BlockHash, IEnumerable<string> TransactionHashes)>(i => LoadTransactionHashesArray(i), options);
            _loadTransactionsBlock = new TransformBlock<(int Index, string BlockHash,
                IEnumerable<string> TransactionHashes), Block>(i => LoadTransactionDetails(i), options);
            _saveInDatabaseBlock = new ActionBlock<Block>(b => SaveBitcoinBlockToDatabase(b), options);
            _logDoubleProcessingBlock = new ActionBlock<(int index, bool isProcessed)>(i => LogProcessedBlock(i.index), options);

            BuildPipeline();
        }

        public async Task StartProcessingAsync(int count)
        {
            await _getBlocksIndexesBlock.SendAsync(count);
        }

        public async Task CompleteAsync()
        {
            _getBlocksIndexesBlock.Complete();
            await _saveInDatabaseBlock.Completion;
            Console.WriteLine($"Processing has completed");
        }

        private void BuildPipeline()
        {
            DataflowLinkOptions options = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            _getBlocksIndexesBlock.LinkTo(_checkIfBlockIsProcessedBlock, options);
            _checkIfBlockIsProcessedBlock.LinkTo(_loadBlockHashBlock, options, i => !i.isProcessed);
            _checkIfBlockIsProcessedBlock.LinkTo(_logDoubleProcessingBlock, options, i => i.isProcessed);
            _loadBlockHashBlock.LinkTo(_loadTransactionHashesBlock, options);
            _loadTransactionHashesBlock.LinkTo(_loadTransactionsBlock, options);
            _loadTransactionsBlock.LinkTo(_saveInDatabaseBlock, options);
        }

        private async Task<IEnumerable<int>> GetLastIndexes(int count)
        {
            try
            {
                var _bitcoinClient = new BitcoinDataProvider();

                Console.WriteLine($"Start processing of {count} blocks");
                var index = await _bitcoinClient.GetBlocksCount();
                var startIndex = Math.Max(index - count + 1, 0);
                return Enumerable.Range(startIndex, count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Enumerable.Range(0, 0);
        }

        private async Task<(int i, string hash)> GetBlockHashByIndex(int index)
        {
            (int i, string hash) result = (index, null);

            try
            {
                var _bitcoinClient = new BitcoinDataProvider();
                Console.WriteLine($"Processing of block with the index {index} has started");
                result.hash = await _bitcoinClient.GetBlocksHash(index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        private async Task<(int Index, string BlockHash, IEnumerable<string> Hashes)> LoadTransactionHashesArray((int Index, string BlockHash) block)
        {
            (int Index, string BlockHash, IEnumerable<string> Hashes) result = (block.Index, block.BlockHash, null);

            try
            {
                if (string.IsNullOrEmpty(block.BlockHash))
                {
                    Console.WriteLine($"Block hash for index {block.Index} is empty");
                    return result;
                }

                var _bitcoinClient = new BitcoinDataProvider();
                Console.WriteLine($"Loading the hashes for the block '{block.BlockHash}'");
                result.Hashes = await _bitcoinClient.GetTransactionHashes(block.BlockHash);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return result;
        }

        private async Task<Block> LoadTransactionDetails((int Index, string BlockHash, IEnumerable<string> TransactionHashes) blockData)
        {
            try
            {
                if (blockData.TransactionHashes == null)
                {
                    return null;
                }

                var _bitcoinClient = new BitcoinDataProvider();
                Console.WriteLine($"Loading transaction details of block {blockData.BlockHash}");
                var transactionHashes = blockData.TransactionHashes.Take(50);
                var transactionDetails = await Task.WhenAll(transactionHashes.Select(async th => await _bitcoinClient.GetTransaction(th)));

                return new Block()
                {
                    Hash = blockData.BlockHash,
                    Index = blockData.Index,
                    Transactions = transactionDetails
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        private async Task SaveBitcoinBlockToDatabase(Block block)
        {
            if (block == null || block.Hash == null)
            {
                Console.WriteLine("Empty bitcoin block");
                return;
            }

            try
            {
                using (BitcoinDbContext context = new BitcoinDbContext())
                {
                    var blockModel = ModelConvertExtension.BuildDomainModel(block);
                    context.Add(blockModel);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task<(int index, bool isProcessed)> CheckIfBlockIsProcessedBlock(int i)
        {
            try
            {
                await Task.CompletedTask;
                using (BitcoinDbContext context = new BitcoinDbContext())
                {
                    if (context.Blocks.Any(b => b.Index == i))
                    {
                        return (i, true);
                    }                       
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return (i, false);
        }

        private void LogProcessedBlock(int index)
        {
            Console.WriteLine($"Block with index {index} was already processed");
        }
    }
}
