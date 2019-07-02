using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometX.Nethereum.Settings
{
    public class NethereumConfig
    {
        #region GENESiS TRANSACTIONS
        public static string TRX_1 = "0xadc9b79f5545b241e9a2feb7620474c7d30c89b8913b077b3c5c1873e0fb6ce3";
        public static string TRX_2 = "0xf693fb8e8d26fde13c7a538167a75791bf0bbce23e0bc3387a69248c8efe0ef2";
        public static string TRX_3 = "0xadc9b79f5545b241e9a2feb7620474c7d30c89b8913b077b3c5c1873e0fb6ce3";
        #endregion

        #region CONTRACTS
        public static string CONTRACT_ADDRESS = "0xC383b524C6F93d6d2aA39175B3e49feE4610e559";
        public static string CONTRACT_API_KEY = "RFV28V3PDE85JSZKYTJPS614VV6RIIUVZ8";

        #region CONTRACT ABI
        public static string ABI = @"[{'constant': true,
        'inputs': [],
        'name': 'name',
        'outputs': [
            {
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_spender',
                'type': 'address'
            },
            {
                'name': '_value',
                'type': 'uint256'
            }
        ],
        'name': 'approve',
        'outputs': [
            {
                'name': 'success',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'totalSupply',
        'outputs': [
            {
                'name': '',
                'type': 'uint256'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_from',
                'type': 'address'
            },
            {
                'name': '_to',
                'type': 'address'
            },
            {
                'name': '_value',
                'type': 'uint256'
            }
        ],
        'name': 'transferFrom',
        'outputs': [
            {
                'name': 'success',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'decimals',
        'outputs': [
            {
                'name': '',
                'type': 'uint8'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_value',
                'type': 'uint256'
            }
        ],
        'name': 'burn',
        'outputs': [
            {
                'name': 'success',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': '_myDecimal',
        'outputs': [
            {
                'name': '',
                'type': 'uint8'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [
            {
                'name': '',
                'type': 'address'
            }
        ],
        'name': 'balanceOf',
        'outputs': [
            {
                'name': '',
                'type': 'uint256'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_from',
                'type': 'address'
            },
            {
                'name': '_value',
                'type': 'uint256'
            }
        ],
        'name': 'burnFrom',
        'outputs': [
            {
                'name': 'success',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': '_myTokeName',
        'outputs': [
            {
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': 'symbol',
        'outputs': [
            {
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_to',
                'type': 'address'
            },
            {
                'name': '_value',
                'type': 'uint256'
            }
        ],
        'name': 'transfer',
        'outputs': [],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': '_mySymbol',
        'outputs': [
            {
                'name': '',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': false,
        'inputs': [
            {
                'name': '_spender',
                'type': 'address'
            },
            {
                'name': '_value',
                'type': 'uint256'
            },
            {
                'name': '_extraData',
                'type': 'bytes'
            }
        ],
        'name': 'approveAndCall',
        'outputs': [
            {
                'name': 'success',
                'type': 'bool'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [],
        'name': '_myinitialSupply',
        'outputs': [
            {
                'name': '',
                'type': 'uint256'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'constant': true,
        'inputs': [
            {
                'name': '',
                'type': 'address'
            },
            {
                'name': '',
                'type': 'address'
            }
        ],
        'name': 'allowance',
        'outputs': [
            {
                'name': '',
                'type': 'uint256'
            }
        ],
        'payable': false,
        'stateMutability': 'view',
        'type': 'function'
    },
    {
        'inputs': [
            {
                'name': 'initialSupply',
                'type': 'uint256'
            },
            {
                'name': 'tokenName',
                'type': 'string'
            },
            {
                'name': 'tokenSymbol',
                'type': 'string'
            }
        ],
        'payable': false,
        'stateMutability': 'nonpayable',
        'type': 'constructor'
    },
    {
        'anonymous': false,
        'inputs': [
            {
                'indexed': true,
                'name': 'from',
                'type': 'address'
            },
            {
                'indexed': true,
                'name': 'to',
                'type': 'address'
            },
            {
                'indexed': false,
                'name': 'value',
                'type': 'uint256'
            }
        ],
        'name': 'Transfer',
        'type': 'event'
    },
    {
        'anonymous': false,
        'inputs': [
            {
                'indexed': true,
                'name': 'from',
                'type': 'address'
            },
            {
                'indexed': false,
                'name': 'value',
                'type': 'uint256'
            }
        ],
        'name': 'Burn',
        'type': 'event'
    }
]";
        #endregion
        #endregion

        #region NETWORK(S)
        // ROPSTEN
        public static string ROPSTEN_API_KEY = "RFV28V3PDE85JSZKYTJPS614VV6RIIUVZ8";
        public static string ROPSTEN_API_URI = "https://api-ropsten.etherscan.io/api";
        public static string ROPSTEN_NETWORK = "https://ropsten.infura.io/2gCBIZ2u8njBJdejbsUa ";

        // ROPSTEN API ACTION METHODS
        public static string ROPSTEN_ACTION_GET_TRX_COUNT = "&action=eth_getTransactionCount"; // - module:proxy
        public static string ROPSTEN_ACTION_SEND_RAW_TRX = "&action=eth_sendRawTransaction"; // - module: proxy
        public static string ROPSTEN_ACTION_TRX_BALANCE = "&action=tokentx"; // - module: account

        // ROPSTEN API MODULES
        public static string ROPSTEN_MODULE_ACCT = "?module=account";
        public static string ROPSTEN_MODULE_PROXY = "?module=proxy";

        // ROPSTEN BUILD URI's
        public static string ROPSTEN_ERC20_TRANSACTIONS(string address, int startBlock = 0, int endBlock = 999999999)
        {
            var baseUri = ROPSTEN_API_URI + "/" + ROPSTEN_MODULE_ACCT;

            var uri = string.Format("{0}&startblock={1}&endblock={2}&sort=asc&apikey=/{3}",
                baseUri, address, startBlock, endBlock, ROPSTEN_API_KEY);

            return uri;
        }

        public static string ROPSTEN_SEND_RAW_TRX(string hash)
        {
            var baseUri = ROPSTEN_API_URI + "/" + ROPSTEN_MODULE_PROXY + ROPSTEN_ACTION_SEND_RAW_TRX;

            var uri = string.Format("{0}&hex={1}&apikey={2}",
                baseUri, ROPSTEN_ACTION_SEND_RAW_TRX, ROPSTEN_API_KEY);

            return uri;
        }
        #endregion

        #region WALLETS 
        // WALKIT 
        public static string WALLET_A = "0xC4609c3b9558e4C833f9065EAEFa0C4AC4D542af";
        public static string WALLET_A_PRIVATE_KEY = "554baf63467416d0159c87af10be1d2f1580180a89a529425434144575a588f2";
        public static string WALLET_A_PW = "";

        // PATIENT
        public static string WALLET_B = "0x188f1ddd60fe72518545864c113f7455fe81b745";
        public static string WALLET_B_PRIVATE_KEY = "b53fcab07acb93781b4e07057878d82d30ec2b3a98bc0ea831bf5cf1886d4ad4";
        public static string WALLET_B_PW = "";

        public static string GetPrivateKey(string address)
        {
            return address.Equals(WALLET_A) ? WALLET_A_PRIVATE_KEY : WALLET_B_PRIVATE_KEY;
        }
        #endregion
    }
}
