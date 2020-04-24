using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{

    public class AbstractResponseApiModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

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


        public ICollection<ItemListApiModel> ListItem { get; set; }


        public TransactionInfo TransactionInfo { get; set; }

        public TransactionInfo[] ListTransactionInfo { get; set; }
        public long BlockId { get; set; }

        public string TransactionId { get; set; }


    }


    public partial class WalletDataResponseApiModel : AbstractResponseApiModel
    {
        public WalletDataResponseApiModel()
        {
            Delegated = new DelegatedStructure();
        }
        public Decimal Balance { get; set; }

        public long LastTransaction { get; set; }

        public DelegatedStructure Delegated { get; set; }

        public string MessageError { get; set; }
    }

    public partial class WalletTransactionsResponseApiModel : AbstractResponseApiModel
    {
        public WalletTransactionsResponseApiModel()
        {
            Transactions = new List<TransactionApiModel>();
        }

        public ICollection<TransactionApiModel> Transactions { get; set; }


    }

    public partial class TransactionApiModel
    {
        public string Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime Time { get; set; }
        public string Sum { get; set; }
        public string Fee { get; set; }
        public string Currency { get; set; }
        public long InnerId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }


    public partial class DelegatedStructure
    {
        public DelegatedStructure()
        {
            Donors = new List<DelegatedInfo>();
            Recipients = new List<DelegatedInfo>();
        }
        public Decimal Incoming { get; set; }

        public Decimal Outgoing { get; set; }

        public ICollection<DelegatedInfo> Donors { get; set; }

        public ICollection<DelegatedInfo> Recipients { get; set; }
    }


    public partial class DelegatedInfo
    {
        public string PublicKey { get; set; }

        public Decimal Sum { get; set; }

        public long ValidUntil { get; set; }
    }
    /// <summary>
    /// Модель одиночный элемент
    /// </summary>
    public class DataResponseApiModel
    {
        public string PublicKey { get; set; }

        /// <summary>
        /// Base58
        /// </summary>
        public string TransactionPackagedStr { get; set; }

        public Decimal ActualSum { get; set; }
        public Decimal RecommendedFee { get; set; }
    }

    public class BalanceResponseApiModel : AbstractResponseApiModel
    {
        public BalanceResponseApiModel()
        {
            Tokens = new List<Token>();
        }

        public Decimal Balance { get; set; }


        public ICollection<Token> Tokens { get; set; }


        public Decimal DelegatedOut { get; set; }

        public Decimal DelegatedIn { get; set; }

    }

    public class TokensResponseApiModel : AbstractResponseApiModel
    {
        public TokensResponseApiModel()
        {
            Tokens = new List<Token>();
        }

        public ICollection<Token> Tokens { get; set; }

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
