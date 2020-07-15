namespace CS.Service.RestApiNode.Models
{
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
}
