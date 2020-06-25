using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class EFeeItem
    {
        public decimal Fee { get; set; }

        public string Comment { get; set; }
    }
    public partial class ResponseApiModel : AbstractResponseApiModel
    {
        public ResponseApiModel()
        {
            DataResponse = new DataResponseApiModel();
            ListItem = new List<ItemListApiModel>();
        }

        public long? TransactionInnerId { get; set; }

        public Decimal Amount { get; set; }

        public DataResponseApiModel DataResponse { get; set; }

        public Decimal ActualSum { get; set; }
        public Decimal ActualFee { get; set; }

        public List<EFeeItem> ExtraFee { get; set; }


        public ICollection<ItemListApiModel> ListItem { get; set; }


        public TransactionInfo TransactionInfo { get; set; }

        public TransactionInfo[] ListTransactionInfo { get; set; }
        public long BlockId { get; set; }

        public string TransactionId { get; set; }


    }

    /// <summary>
    /// Модель элемент списка
    /// </summary>
    public class ItemListApiModel
    {
        public string PublicKey { get; set; }

        public bool IsCs { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public Decimal Amount { get; set; }

    }

    public class Token
    {
        public string PublicKey { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public Decimal Amount { get; set; }

    }

    public class FilteredTransactionsResponseModel : AbstractResponseApiModel
    {
        public FilteredTransactionsResponseModel()
        {
            QuerieResponses = new List<QueryResponseItem>();
        }

        public ICollection<QueryResponseItem> QuerieResponses { get; set; }
    }
    public class QueryResponseItem
    {
        public QueryResponseItem()
        {
            Transactions = new List<ShortTransactionInfo>();
            TransfersList = new List<QueryTokenResponseItem> ();
        }
        public string QueryAddress { get; set; }

        public ICollection<ShortTransactionInfo> Transactions { get; set; }

        public ICollection<QueryTokenResponseItem> TransfersList { get; set; }
    }

    public class QueryTokenResponseItem
    {
        public QueryTokenResponseItem()
        {
            Transfers = new List<TokenTransferInfo>();
        }
        public string TokenAddress { get; set; }
        public string TokenName { get; set; }
        public string TokenTiker { get; set; }

        public ICollection<TokenTransferInfo> Transfers { get; set; }
    }

    public class ShortTransactionInfo
    {
        public decimal Amount { get; set; }
        public UInt16 Currency { get; set; }
        public decimal Fee { get; set; }
        public string TransactionId { get; set; }
        /// <summary>
        /// Base58
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Base58
        /// </summary>
        public string Target { get; set; }
        public DateTime TimeCreation { get; set; }
        public Byte Type { get; set; }
    }

    public class TokenTransferInfo
    {
        public string TokenAddress { get; set; }

        public string TokenCode { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public Decimal TokenAmount { get; set; }

        public string TransferInitiator { get; set; }

        public string TransactionID { get; set; }

        public DateTime TimeCreation { get; set; }
    }

    public class TransactionInfo : AbstractResponseApiModel
    {
        public string Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public string Fee { get; set; }
        public string Currency { get; set; }
        public long InnerId { get; set; }
        public long Index { get; set; }
        public string Status { get; set; }
        public string BlockNum
        {
            get
            {
                if (Id == null || !Id.Contains(".")) return null;
                return Id.Split(".")[0];
            }
        }
        public bool Found { get; set; }
        public string UserData { get; set; }

        public List<TxProperty> Props = new List<TxProperty>();
        public string Signature { get; set; }
        public List<TxFee> ExtraFee { get; set; }

        public Extra Bundle { get; set; }

    }

    public class Extra
    {
        public SmartContractModel Contract { get; set; }

        public SmartInfo ContractInfo { get; set; }

    }


    public class SmartContractModel
    {
        public bool ForgetNewState { get; set; }

        public SmartContractExecuteModel Execute { get; set; }

        public SmartContractDeployModel Deploy { get; set; }

    }

    public class SmartContractExecuteModel
    {
        public SmartContractExecuteModel()
        {
            Params = new List<string>();
        }
        public string Method { get; set; }

        public List<string> Params { get; set; }
    }


    public class SmartContractDeployModel
    {
        public string SourceCode { get; set; }
        public int TokenStandard { get; set; }
        
        public string HashState { get; set; }
    }

    public class SmartContractMethodsModel : AbstractResponseApiModel
    {
        public SmartContractMethodsModel()
        {
            Methods = new List<ContractMethod>();
        }
        public ICollection<ContractMethod> Methods { get; set; }
    }

    public class ContractMethod
    {
        public ContractMethod()
        {
            Arguments = new List<ModelMethodArgument> ();
            Annotations = new List<ModelAnnotation>();
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }

        public ICollection<ModelMethodArgument> Arguments { get; set; }

        public ICollection<ModelAnnotation> Annotations { get; set; }
    }

    public class ModelMethodArgument
    {
        public ModelMethodArgument()
        {
            Annotations = new List<ModelAnnotation>();
        }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<ModelAnnotation> Annotations { get; set; }
    }

    public class ModelAnnotation
    {
        public ModelAnnotation()
        {
            Arguments = new Dictionary<string, string>();
        }
        public string Name { get; set; }

        public IDictionary<string, string> Arguments { get; set; }

    }

    public class SmartInfo
    {
        //SmartDeploy  { get; set; }
        //SmartExecution  { get; set; }
    }

    public class SmartSourceCode : AbstractResponseApiModel
    {
        public string gZipped { get; set; }
        //public ICollection<byte> gZipped { get; set; }

        public string SourceString { get; set; }
    }

    //std::string sourceCode;
    //std::vector< ::general::ByteCodeObject> byteCodeObjects;
    //std::string hashState;
    //int32_t tokenStandard;

    public class ContractValidationResponse : AbstractResponseApiModel
    {
        public ContractDeploy Deploy { get; set; }

        public int TokenStandard { get; set; }
    }
    public class ContractDeploy
    {
        public ContractDeploy()
        {
            ByteCodeObjects = new List<BCObject>();
        }
        public string SourceCode { get; set; }

        public ICollection<BCObject> ByteCodeObjects { get; set; }
    }

    public class BCObject
    {
        public string Name { get; set; }
        public ICollection<byte> ByteCode { get; set; }
    }

    public class TxProperty
    {
        public string Title;
        public string Value;
        public bool IsLink;
        public bool IsCode;

        public TxProperty(string title, string value, bool link = false, bool code = false)
        {
            Title = title;
            Value = value;
            IsLink = link;
            IsCode = code;
        }

    }

    public class TxFee
    {
        public string Fee;
        public string Comment;

        public TxFee(string fee, string comment)
        {
            Fee = fee;
            Comment = comment;
        }
    }
}
