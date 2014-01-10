using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Collections;

namespace App55 {
    public sealed class Gateway {
        private Environment environment;
        private string apiKey;
        private string apiSecret;
        private ICredentials credentials;

        public Gateway(Environment environment, string apiKey, string apiSecret) {
            this.environment = environment;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        public Environment Environment {
            get {
                return environment;
            }
        }

        public string ApiKey {
            get {
                return apiKey;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string ApiSecret {
            get {
                return apiSecret;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ICredentials Credentials {
            get {
                if(credentials == null) {
                    credentials = new NetworkCredential(this.apiKey, this.apiSecret); 
                }
                return credentials;
            }
        }

        public QueryStringResponseFactory Response {
            get {
                return new QueryStringResponseFactory(this);
            }
        }

        public CardCreateRequest CreateCard(User user, Card card) {
            CardCreateRequest request = new CardCreateRequest(user, card);
            request.Gateway = this;
            return request;
        }

        public CardCreateRequest CreateCard(User user, Card card, bool threeds) {
            CardCreateRequest request = new CardCreateRequest(user, card, threeds);
            request.Gateway = this;
            return request;
        }

        public CardDeleteRequest DeleteCard(User user, Card card) {
            CardDeleteRequest request = new CardDeleteRequest(user, card);
            request.Gateway = this;
            return request;
        }

        public CardListRequest ListCards(User user) {
            CardListRequest request = new CardListRequest(user);
            request.Gateway = this;
            return request;
        }

        public TransactionCreateRequest CreateTransaction(User user, Card card, Transaction transaction) {
            TransactionCreateRequest request = new TransactionCreateRequest(user, card, transaction);
            request.Gateway = this;
            return request;
        }

        public TransactionCreateRequest CreateTransaction(User user, Card card, Transaction transaction, bool threeds) {
            TransactionCreateRequest request = new TransactionCreateRequest(user, card, transaction, threeds);
            request.Gateway = this;
            return request;
        }

        public TransactionCreateRequest CreateTransaction(Card card, Transaction transaction) {
            TransactionCreateRequest request = new TransactionCreateRequest(card, transaction);
            request.Gateway = this;
            return request;
        }

        public TransactionCommitRequest CommitTransaction(Transaction transaction) {
            TransactionCommitRequest request = new TransactionCommitRequest(transaction);
            request.Gateway = this;
            return request;
        }

        public UserCreateRequest CreateUser(User user) {
            UserCreateRequest request = new UserCreateRequest(user);
            request.Gateway = this;
            return request;
        }

        public UserAuthenticateRequest AuthenticateUser(User user) {
            UserAuthenticateRequest request = new UserAuthenticateRequest(user);
            request.Gateway = this;
            return request;
        }

        public UserUpdateRequest UpdateUser(User user) {
            UserUpdateRequest request = new UserUpdateRequest(user);
            request.Gateway = this;
            return request;
        }
    }

    public sealed class QueryStringResponseFactory {
        private Gateway gateway;

        internal QueryStringResponseFactory(Gateway gateway) {
            this.gateway = gateway;
        }
        private T CreateResponse<T>(string queryString) where T : Response, new() {
            

            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            String[] parts = queryString.Split('&');
            foreach(String part in parts) {
                String[] param = part.Split(new char[] { '=' }, 2);
                dictionary[Uri.UnescapeDataString(param[0])] = Uri.UnescapeDataString(param[1]);
            }

            Hashtable message = new Hashtable();
            foreach(KeyValuePair<string, string> entry in dictionary) {
                String[] path = entry.Key.Split('.');
                Hashtable part = message;
                for(int i = 0; i < path.Length - 1; i++) {
                    if(!typeof(Hashtable).IsInstanceOfType(part[path[i]]))
                        part[path[i]] = new Hashtable();
                    part = (Hashtable)part[path[i]];
                }
                part[path[path.Length - 1]] = entry.Value;
            }

            if(message["error"] != null) {
                throw ApiException.CreateException((Hashtable)message["error"]);
            } else {
                T response = new T();
                response.Gateway = this.gateway;
                response.Populate(message);

                if(!response.IsValidSignature) throw new InvalidSignatureException();
                return response;
            }
        }

        public CardCreateResponse CreateCard(string queryString) {
            return CreateResponse<CardCreateResponse>(queryString);
        }

        public CardListResponse ListCards(string queryString) {
            return CreateResponse<CardListResponse>(queryString);
        }

        public CardDeleteResponse DeleteCards(string queryString) {
            return CreateResponse<CardDeleteResponse>(queryString);
        }

        public TransactionCreateResponse CreateTransaction(string queryString) {
            return CreateResponse<TransactionCreateResponse>(queryString);
        }

        public TransactionCommitResponse CommitTransaction(string queryString) {
            return CreateResponse<TransactionCommitResponse>(queryString);
        }

        public UserCreateResponse CreateUser(string queryString) {
            return CreateResponse<UserCreateResponse>(queryString);
        }

        public UserUpdateResponse UpdateUser(string queryString) {
            return CreateResponse<UserUpdateResponse>(queryString);
        }

        public UserAuthenticateResponse AuthenticateUser(string queryString) {
            return CreateResponse<UserAuthenticateResponse>(queryString);
        }
    }

    public sealed class Environment {
        private string server;
        private int port;
        private bool isSsl;
        private int version;

        private Environment(string server, int port, bool isSsl, int version) {
            this.server = server;
            this.port = port;
            this.isSsl = isSsl;
            this.version = version;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string BaseUrl {
            get {
                return this.Scheme + "://" + this.Host + "/v" + this.version;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal bool IsSSL {
            get {
                return this.isSsl;
            }
        }

        private string Scheme {
            get {
                return this.isSsl ? "https" : "http";
            }
        }

        private string Host {
            get {
                if(this.port == 443 && this.isSsl)
                    return this.server;
                else if(this.port == 80 && !this.isSsl)
                    return this.server;
                else
                    return this.server + ":" + this.port;
            }
        }

        public static readonly Environment Development = new Environment("dev.app55.com", 80, false, 1);
        public static readonly Environment Sandbox = new Environment("sandbox.app55.com", 443, true, 1);
        public static readonly Environment Production = new Environment("api.app55.com", 443, true, 1);
    }
}
