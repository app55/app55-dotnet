using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections;
using System.ComponentModel;

namespace App55 {
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Message {
 
        private Gateway gateway;

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Message() {

        }

        [IgnoreProperty]
        public Gateway Gateway {
            get {
                return this.gateway;
            }
            [EditorBrowsable(EditorBrowsableState.Never)]
            internal set {
                this.gateway = value;
            }
        }

        [PropertyName("sig")]
        public abstract string Signature {
            get;
            set;
        }

        [PropertyName("ts")]
        public abstract string Timestamp {
            get;
            set;
        }

        [IgnoreProperty]
        public string FormData {
            get {
                return ToFormData();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal protected string ToSignature(bool includeApiKey = false) {
            IDictionary<string, bool> exclude = new Dictionary<string, bool>();
            exclude.Add("sig", true);
            string formData = ToFormData(includeApiKey, exclude);
            SHA1 sha = new SHA1CryptoServiceProvider();
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] digest = sha.ComputeHash(encoding.GetBytes(this.Gateway.ApiSecret + formData));
            return Convert.ToBase64String(digest).Replace('+', '-').Replace('/', '_');
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal protected string ToFormData(bool includeApiKey = true, IDictionary<string, bool> exclude = null) {
            string formData = "";
            IDictionary<string, string> description = Describe(this, null, exclude);
            if(includeApiKey) {
                description.Add("api_key", this.Gateway.ApiKey);
                if(exclude == null || !exclude.ContainsKey("sig")) {
                    description.Remove("sig");
                    description.Add("sig", ToSignature(true));
                }
            }
            foreach(KeyValuePair<string, string> entry in description) {
                formData += "&" + UriBuilder.Encode(entry.Key) + "=" + UriBuilder.Encode(entry.Value);
            }
            if(formData.Length > 0) formData = formData.Substring(1);
            return formData;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal protected IDictionary<string, string> Describe(object o, string prefix = null, IDictionary<string, bool> exclude = null) {
            if(o == null) return new Dictionary<string, string>();
            prefix = prefix == null ? "" : prefix + ".";

            IDictionary<string, string> description = new SortedDictionary<string, string>();
            foreach(PropertyInfo prop in o.GetType().GetProperties()) {
                if(Attribute.IsDefined(prop, typeof(IgnoreProperty))) continue;

                string name = prop.Name.ToLower();
                if(Attribute.IsDefined(prop, typeof(PropertyName)))
                    name = ((PropertyName)Attribute.GetCustomAttribute(prop, typeof(PropertyName))).Name;

                if(exclude != null && exclude.ContainsKey(name)) continue;

                object value = prop.GetValue(o, null);
                if(value == null) continue;

                if(Converter.Instance.CanConvert(prop.PropertyType)) {
                    description[prefix + name] = Converter.Instance.Convert(value);
                } else if(prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)) {
                    IList v = (IList)value;
                    int i = 0;
                    foreach(object obj in v) {
                        foreach(KeyValuePair<string, string> entry in Describe(obj, prefix + name + "." + i))
                            description.Add(entry);
                        i++;
                    }
                } else {
                    foreach(KeyValuePair<string, string> entry in Describe(value, prefix + name))
                        description.Add(entry);
                }
            }
            return description;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property)]
    internal class IgnoreProperty : System.Attribute {

    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property)]
    internal class PropertyName : System.Attribute {
        private string name;

        public PropertyName(string name) {
            this.name = name;
        }

        public string Name {
            get {
                return this.name;
            }
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Request<T> : Message where T : Response, new() {

        internal Request() {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreProperty]
        internal abstract string HttpEndpoint {
            get;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [IgnoreProperty]
        internal virtual string HttpMethod {
            get {
                return "GET";
            }
        }

        public override string Signature {
            get {
                return ToSignature(true);
            }
            set {

            }
        }

        public override string Timestamp {
            get {
                return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            }
            set {

            }
        }

        public T Send() {
            UTF8Encoding encoding = new UTF8Encoding();

            HttpWebRequest request;
            IDictionary<string, bool> exclude = new Dictionary<string, bool>();
            exclude.Add("sig", true);
            exclude.Add("ts", true);
            string qs = ToFormData(false, exclude);

            if(this.HttpMethod == "GET") {
                request = (HttpWebRequest)WebRequest.Create(this.HttpEndpoint + "?" + qs);
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(encoding.GetBytes(this.Gateway.Credentials.GetCredential(new Uri(this.HttpEndpoint), "Basic").UserName + ":" + this.Gateway.Credentials.GetCredential(new Uri(this.HttpEndpoint), "Basic").Password)));

            } else {
                byte[] data = encoding.GetBytes(qs);
                
                request = (HttpWebRequest)WebRequest.Create(this.HttpEndpoint);
                request.Method = this.HttpMethod;
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(encoding.GetBytes(this.Gateway.Credentials.GetCredential(new Uri(this.HttpEndpoint), "Basic").UserName + ":" + this.Gateway.Credentials.GetCredential(new Uri(this.HttpEndpoint), "Basic").Password)));
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            HttpWebResponse response;
            try {
                response = (HttpWebResponse)request.GetResponse();
            } catch(WebException e) {
                response = (HttpWebResponse)e.Response;
            }

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string json = reader.ReadToEnd();
            response.Close();

            Hashtable ht = (Hashtable)JSON.Parse(json);

            if(ht.ContainsKey("error")) throw ApiException.CreateException((Hashtable)ht["error"]);

            T r = new T();
            r.Gateway = this.Gateway;
            r.Populate(ht);
            if(!r.IsValidSignature) throw new InvalidSignatureException();
            return r;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Response : Message {
        private string signature;
        private string timestamp;

        internal Response() {

        }

        public override string Signature {
            get {
                return this.signature;
            }
            set {
                this.signature = value;
            }
        }

        public override string Timestamp {
            get {
                return this.timestamp;
            }
            set {
                this.timestamp = value;
            }
        }

        [IgnoreProperty]
        public bool IsValidSignature {
            get {
                return ToSignature() == this.signature;
            }
        }

        internal void Populate(Hashtable hashtable, object o = null) {
            if(o == null) o = this;

            IDictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
            foreach(PropertyInfo prop in o.GetType().GetProperties()) {
                if(prop.GetSetMethod(true) == null) continue;

                string name = prop.Name.ToLower();
                if(Attribute.IsDefined(prop, typeof(PropertyName)))
                    name = ((PropertyName)Attribute.GetCustomAttribute(prop, typeof(PropertyName))).Name;

                properties.Add(name, prop);
            }

            foreach(DictionaryEntry entry in hashtable) {
                string key = (string)entry.Key;
                object value = entry.Value;

                if (value == null || !properties.ContainsKey(key)) {
                    continue;
                }

                if(value is Hashtable) {
                    if(properties[key].GetValue(o, null) == null)
                        properties[key].SetValue(o, Activator.CreateInstance(properties[key].PropertyType), null);
                    Populate((Hashtable)value, properties[key].GetValue(o, null));
                } else if(value is ArrayList) {
                    if(properties[key].PropertyType.GetGenericTypeDefinition() == typeof(IList<>)) {
                        Type argument = properties[key].PropertyType.GetGenericArguments()[0];
                        Type generic = typeof(List<>).MakeGenericType(new Type[] { argument });
                        IList list = (IList)Activator.CreateInstance(generic);
                        foreach(object arrayItem in ((ArrayList)value)) {
                            if(arrayItem is Hashtable) {
                                object listItem = Activator.CreateInstance(argument);
                                Populate((Hashtable)arrayItem, listItem);
                                list.Add(listItem);
                            } else {
                                list.Add(arrayItem);
                            }
                        }
                        properties[key].SetValue(o, list, null);
                    }

                } else {
                    properties[key].SetValue(o, Converter.Instance.Convert(value.ToString(), properties[key].PropertyType), null);
                }
            }
        }
    }




    public sealed class CardCreateRequest : Request<CardCreateResponse> {
        private User user;
        private Card card;
        private String ipAddress;

        internal CardCreateRequest(User user, Card card) {
            this.user = user;
            this.card = card;
        }

        internal override string HttpEndpoint {
            get { 
                return this.Gateway.Environment.BaseUrl + "/card";
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        public Card Card {
            get {
                return this.card;
            }
            set {
                this.card = value;
            }
        }

        [PropertyName("ip_address")]
        public String IPAddress {
            get {
                return this.ipAddress;
            }
            set {
                this.ipAddress = value;
            }
        }
    }

    public sealed class CardCreateResponse : Response {

        private Card card;

        public Card Card {
            get {
                return this.card;
            }
            internal set {
                this.card = value;
            }
        }
    }

    public sealed class CardDeleteRequest : Request<CardDeleteResponse> {
        private User user;
        private Card card;

        public CardDeleteRequest(User user, Card card) {
            this.user = user;
            this.card = card;
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        [IgnoreProperty]
        public Card Card {
            get {
                return this.card;
            }
            set {
                this.card = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/card/" + this.Card.Token;
            }
        }

        internal override string HttpMethod {
            get {
                return "DELETE";
            }
        }
    }

    public sealed class CardDeleteResponse : Response {

    }

    public sealed class CardListRequest : Request<CardListResponse> {
        private User user;

        public CardListRequest(User user) {
            this.user = user;
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/card";
            }
        }
    }

    public sealed class CardListResponse : Response {
        private IList<Card> cards;

        public IList<Card> Cards {
            get {
                return this.cards;
            }
            internal set {
                this.cards = value;
            }
        }
    }

    public sealed class TransactionCommitRequest : Request<TransactionCommitResponse> {
        private Transaction transaction;

        public TransactionCommitRequest(Transaction transaction) {
            this.transaction = transaction;
        }

        [IgnoreProperty]
        public Transaction Transaction {
            get {
                return this.transaction;
            }
            set {
                this.transaction = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/transaction/" + this.Transaction.ID;
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }
    }

    public sealed class TransactionCommitResponse : Response {
        private Transaction transaction;

        public Transaction Transaction {
            get {
                return this.transaction;
            }
            internal set {
                this.transaction = value;
            }
        }
    }

    public sealed class TransactionCreateRequest : Request<TransactionCreateResponse> {
        private User user;
        private Transaction transaction;
        private Card card;
        private String ipAddress;

        public TransactionCreateRequest(User user, Card card, Transaction transaction) {
            this.user = user;
            this.card = card;
            this.transaction = transaction;
        }

        public TransactionCreateRequest(Card card, Transaction transaction) : this(null, card, transaction) {

        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        public Card Card {
            get {
                return this.card;
            }
            set {
                this.card = value;
            }
        }

        public Transaction Transaction {
            get {
                return this.transaction;
            }
            set {
                this.transaction = value;
            }
        }

        [PropertyName("ip_address")]
        public String IPAddress {
            get {
                return this.ipAddress;
            }
            set {
                this.ipAddress = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/transaction";
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }
    }

    public sealed class TransactionCreateResponse : Response {
        private Transaction transaction;

        public Transaction Transaction {
            get {
                return this.transaction;
            }
            internal set {
                this.transaction = value;
            }
        }
    }

    public sealed class UserAuthenticateRequest : Request<UserAuthenticateResponse> {
        private User user;

        public UserAuthenticateRequest(User user) {
            this.user = user;
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/user/authenticate";
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }
    }

    public sealed class UserAuthenticateResponse : Response {
        private User user;

        public User User {
            get {
                return this.user;
            }
            internal set {
                this.user = value;
            }
        }
    }

    public sealed class UserCreateRequest : Request<UserCreateResponse> {
        private User user;

        public UserCreateRequest(User user) {
            this.user = user;
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/user";
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }
    }

    public sealed class UserCreateResponse : Response {
        private User user;

        public User User {
            get {
                return this.user;
            }
            internal set {
                this.user = value;
            }
        }
    }

    public sealed class UserUpdateRequest : Request<UserUpdateResponse> {
        private User user;

        public UserUpdateRequest(User user) {
            this.user = user;
        }

        public User User {
            get {
                return this.user;
            }
            set {
                this.user = value;
            }
        }

        internal override string HttpEndpoint {
            get {
                return this.Gateway.Environment.BaseUrl + "/user/" + this.User.ID;
            }
        }

        internal override string HttpMethod {
            get {
                return "POST";
            }
        }
    }

    public sealed class UserUpdateResponse : Response {
        private User user;

        public User User {
            get {
                return this.user;
            }
            internal set {
                this.user = value;
            }
        }
    }
}
