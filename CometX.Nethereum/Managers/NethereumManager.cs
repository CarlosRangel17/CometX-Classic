using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.StandardTokenEIP20;
using CometX.Nethereum.Settings;
using CometX.Nethereum.Models;
using CometX.Nethereum.Extensions;

namespace CometX.Nethereum.Managers
{
    public class NethereumManager
    {
        #region global variable(s)
        private readonly string _abi;
        private readonly Web3 _web3;
        private readonly Contract _contract;
        #endregion

        #region constructor(s)
        public NethereumManager()
        {
            _abi = NethereumConfig.ABI;
            _web3 = new Web3(NethereumConfig.ROPSTEN_NETWORK);
            _contract = _web3.Eth.GetContract(_abi, NethereumConfig.CONTRACT_ADDRESS);
        }

        public NethereumManager(Web3 web3, string address, string abi)
        {
            _abi = abi;
            _web3 = web3;
            _contract = web3.Eth.GetContract(_abi, address);
        }
        #endregion

        #region public methods
        public async Task<decimal> GetETHBalance(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            var etherAmount = Web3.Convert.FromWei(balance.Value);
            return etherAmount;
        }

        public async Task<decimal> GetERC20Balance(string address)
        {
            var standardToken = new StandardTokenService(_web3, NethereumConfig.CONTRACT_ADDRESS);
            // TODO: Test BalanceOfQueryAsync method 
            var balance = await standardToken.BalanceOfQueryAsync(NethereumConfig.WALLET_B);
            return Web3.Convert.FromWei(balance);
        }

        public async Task<decimal> GetNumberOfTransactions(string address)
        {
            var balance = await _web3.Eth.GetBalance.SendRequestAsync(address);
            var hexCount = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(address);
            var count = Web3.Convert.FromWei(hexCount.Value);
            return count;
        }

        public async Task<RopstenRequest> GetRopstenERC20Transactions()
        {
            var url = NethereumConfig.ROPSTEN_ERC20_TRANSACTIONS(NethereumConfig.WALLET_B);
            var request = await url.GetAsyncRopstenRequest();
            return request;
        }

        public async Task<String> SendOfflineERC20Token(string fromWallet, string toWallet, int tokenAmount, int maxNonce = 500)
        {
            var hexAmount = tokenAmount.DecimalToHexadecimal();
            var functionInput = new object[] { toWallet, hexAmount };
            var transferFunction = _contract.GetFunction("transfer");
            var data = transferFunction.GetData(functionInput);
            var txCount = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(NethereumConfig.CONTRACT_ADDRESS);

            var trxId = "";
            var message = "";
            var nonce = txCount.Value;
            while (nonce < maxNonce)
            {
                try
                {
                    var encoded = Web3.OfflineTransactionSigner
                        .SignTransaction(
                            NethereumConfig.GetPrivateKey(fromWallet), // SIGN WITH PRIVATE KEY
                            NethereumConfig.CONTRACT_ADDRESS,  // SEND TRX TO CONTRACT
                            0, // VALUE OF ETH - In this case, we aren't transferring ETH 
                            nonce, // NONCE
                            new HexBigInteger("400000"), // GAS PRICE
                            new HexBigInteger("400000"), // GAS LIMIT
                            data // CONTRACT DATA
                        );

                    // SIGN TRANSACTION
                    var verifyAddress = Web3.OfflineTransactionSigner.GetSenderAddress(encoded);
                    trxId = await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);
                    break;
                }
                catch (Exception ex)
                {
                    message = ex.Message;

                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        message += "\n" + ex.Message;
                    }

                    if (!message.Contains("nonce") && !message.Contains("underpriced")) throw new Exception(message);

                    nonce++;
                }
            }

            return trxId;
        }

        public async Task<String> SendOfflineETH(int tokenAmount)
        {
            var txCount = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(NethereumConfig.WALLET_B);
            var encoded = Web3.OfflineTransactionSigner.SignTransaction(NethereumConfig.WALLET_B_PRIVATE_KEY, NethereumConfig.WALLET_A, tokenAmount, txCount.Value);
            var verifyAddress = Web3.OfflineTransactionSigner.GetSenderAddress(encoded);
            var txId = await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);
            return txId;
        }

        // Create A Transaction 
        //https://stackoverflow.com/questions/44039721/functioncall-callasyncstring-in-nethereum-returning-null
        public async Task<object> VerifyTRXInfo(string trxHash, bool? overrideFlag = null)
        {
            bool success = false;
            string message = "";
            var receipt = new TransactionReceipt();

            try
            {
                // Web3
                receipt = await _web3.TransactionManager.TransactionReceiptService.PollForReceiptAsync(trxHash);

                // Contract
                receipt = await _contract.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trxHash);
            }
            catch (Exception ex)
            {
                message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += "\n" + ex.Message;
                }
            }

            var data = new
            {
                success,
                message,
                receipt
            };

            return data;
        }
        #endregion       
    }
}
