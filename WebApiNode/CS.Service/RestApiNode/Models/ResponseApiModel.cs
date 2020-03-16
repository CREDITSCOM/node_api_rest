using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public partial class ResponseApiModel
    {
        public ResponseApiModel()
        {
            DataResponse = new DataResponseApiModel();
            ListItem = new List<ItemListApiModel>();
        }

        public long? TransactionInnerId { get; set; }


        public bool Success { get; set; }

        public string Message { get; set; }

        public string MessageError { get; set; }

        public Decimal Amount { get; set; }

        public DataResponseApiModel DataResponse { get; set; }


        public ICollection<ItemListApiModel> ListItem { get; set; }


        public TransactionInfo TransactionInfo { get; set; }

        public TransactionInfo[] ListTransactionInfo { get; set; }
        public long BlockId { get; set; }

        public string TransactionId { get; set; }
       
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

        public string AmountAsString { get; set; }

        public Decimal DelegatedOut { get; set; }
        
        public string DelegatedOutAsString { get; set; }

        public Decimal DelegatedIn { get; set; }

        public string DelegatedInAsStirng { get; set; }

        public string DelegatedTerm { get; set; }

    }


    public class TransactionInfo
    {
        public string Id { get; set; }
        public string FromAccount{ get; set; }
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
