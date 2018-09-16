﻿using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinScraperLib
{
    public class BitcoinHttpClient
    {
        private const string BlockCountUrl = "https://blockexplorer.com/api/status?q=getblockcount";
        private const string BlockHashUrl = "https://blockexplorer.com/api/block-index/";
        private const string BlockDetailsUrl = "https://blockexplorer.com/api/block/";
        private const string TransactionDetailsUrl = "https://blockexplorer.com/api/tx/";


        private static HttpClient _httpClient = new HttpClient();

        public async Task<string> GetBlocksCountContent()
        {
            return await GetRequestContent(BlockCountUrl);
        }

        public async Task<string> GetBlocksHashContent(int index)
        {
            return await GetRequestContent(BlockHashUrl + index);
        }

        public async Task<string> GetTransactionHashesContent(string hash)
        {
            return await GetRequestContent(BlockDetailsUrl + hash);
        }

        public async Task<string> GetTransactionContent(string transactionHash)
        {
            return await GetRequestContent(TransactionDetailsUrl + transactionHash);
        }

        private async Task<string> GetRequestContent(string url)
        {
            var blockCountRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(blockCountRequest);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
