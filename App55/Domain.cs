using System;
using System.Collections.Generic;
using System.Text;

namespace App55 {
    public sealed class User {
        private long? id;
        private string email;
        private string password;
        private string confirmPassword;
        private string phone;

        public User() {

        }

        public User(long id) {
            this.ID = id;
        }

        public User(string email) {
            this.Email = email;
        }

        public User(string email, string password) {
            this.Email = email;
            this.Password = password;
        }

        public User(string email, string phone, string password, string confirmPassword) {
            this.Email = email;
            this.Phone = phone;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public User(string email, string password, string confirmPassword) {
            this.Email = email;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public User(long id, string email) {
            this.ID = id;
            this.Email = email;
        }

        public User(long id, string password, string confirmPassword) {
            this.ID = id;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public User(long id, string email, string password, string confirmPassword) {
            this.ID = id;
            this.Email = email;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public long? ID {
            get {
                return this.id;
            }
            set {
                if(value != null && value < 1L) throw new ArgumentOutOfRangeException();
                this.id = value;
            }
        }

        public string Email {
            get {
                return this.email;
            }
            set {
                this.email = value;
            }
        }

        public string Password {
            get {
                return this.password;
            }
            set {
                this.password = value;
            }
        }

        [PropertyName("password_confirm")]
        public string ConfirmPassword {
            get {
                return this.confirmPassword;
            }
            set {
                this.confirmPassword = value;
            }
        }

        public string Phone {
            get {
                return this.phone;
            }
            set {
                this.phone = value;
            }
        }
    }

    public sealed class Transaction {
        private string id;
        private string amount;
        private string currency;
        private string description;
        private string code;
        private string authCode;
        private bool? commit;

        public Transaction() {

        }

        public Transaction(string amount, string currency, string description) {
            this.Amount = amount;
            this.Currency = currency;
            this.Description = description;
        }

        public Transaction(string id, string amount, string currency, string description)
            : this(amount, currency, description) {
            this.ID = id;
        }

        public Transaction(string id) {
            this.ID = id;
        }

        public string ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }

        public string Amount {
            get {
                return this.amount;
            }
            set {
                this.amount = value;
            }
        }

        public string Currency {
            get {
                return this.currency;
            }
            set {
                this.currency = value;
            }
        }

        public string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
            }
        }

        public string Code {
            get {
                return this.code;
            }
            set {
                this.code = value;
            }
        }

        [PropertyName("auth_code")]
        public string AuthCode {
            get {
                return this.authCode;
            }
            set {
                this.authCode = value;
            }
        }

        public bool? Commit {
            get {
                return this.commit;
            }
            set {
                this.commit = value;
            }
        }
    }

    public sealed class Card {
        private String token;

        private string holderName;
        private string number;
        private string expiry;
        private string expiryMonth;
        private string expiryYear;
        private string securityCode;
        private Address address;
        private string type;
        private string description;
        private string issue;

        public Card() {

        }

        public Card(string token) {
            this.Token = token;
        }

        public Card(string holderName, string number, string expiry, string securityCode, string issue, Address address) {
            this.HolderName = holderName;
            this.Number = number;
            this.Expiry = expiry;
            this.SecurityCode = securityCode;
            this.Address = address;
            this.Issue = issue;
        }

        public Card(string holderName, string number, string expiry, string securityCode, string issue, Address address, string description)
            : this(holderName, number, expiry, securityCode, issue, address) {
            this.Description = description;
        }

        public Card(string holderName, string number, string expiryMonth, string expiryYear, string securityCode, string issue, Address address) {
            this.HolderName = holderName;
            this.Number = number;
            this.ExpiryMonth = expiryMonth;
            this.ExpiryYear = expiryYear;
            this.SecurityCode = securityCode;
            this.Address = address;
            this.Issue = issue;
        }

        public Card(string holderName, string number, string expiryMonth, string expiryYear, string securityCode, string issue, Address address, string description)
            : this(holderName, number, expiryMonth, expiryYear, securityCode, issue, address) {
            this.Description = description;
        }

        public string Token {
            get {
                return this.token;
            }
            set {
                this.token = value;
            }
        }

        [PropertyName("holder_name")]
        public string HolderName {
            get {
                return this.holderName;
            }
            set {
                this.holderName = value;
            }
        }

        public string Number {
            get {
                return this.number;
            }
            set {
                this.number = value;
            }
        }

        public string Expiry {
            get {
                return this.expiry;
            }
            set {
                this.expiry = value;
            }
        }

        [PropertyName("expiry_month")]
        public string ExpiryMonth {
            get {
                return this.expiryMonth;
            }
            set {
                this.expiryMonth = value;
            }
        }

        [PropertyName("expiry_year")]
        public string ExpiryYear {
            get {
                return this.expiryYear;
            }
            set {
                this.expiryYear = value;
            }
        }

        [PropertyName("security_code")]
        public string SecurityCode {
            get {
                return this.securityCode;
            }
            set {
                this.securityCode = value;
            }
        }

        public Address Address {
            get {
                return this.address;
            }
            set {
                this.address = value;
            }
        }

        public string Type {
            get {
                return this.type;
            }
            set {
                this.type = value;
            }
        }

        public string Description {
            get {
                return this.description;
            }
            set {
                this.description = value;
            }
        }

        public string Issue {
            get {
                return this.issue;
            }
            set {
                this.issue = value;
            }
        }
    }

    public sealed class Address {
        private string street;
        private string street2;
        private string city;
        private string postalCode;
        private string country;

        public Address() {

        }

        public Address(string street, string city, string postalCode, string country) {
            this.Street = street;
            this.City = city;
            this.PostalCode = postalCode;
            this.Country = country;
        }

        public Address(string street, string street2, string city, string postalCode, string country)
            : this(street, city, postalCode, country) {
            this.Street2 = street2;
        }

        public string Street {
            get {
                return this.street;
            }
            set {
                this.street = value;
            }
        }

        public string Street2 {
            get {
                return this.street2;
            }
            set {
                this.street2 = value;
            }
        }

        public string City {
            get {
                return this.city;
            }
            set {
                this.city = value;
            }
        }

        [PropertyName("postal_code")]
        public string PostalCode {
            get {
                return this.postalCode;
            }
            set {
                this.postalCode = value;
            }
        }

        public string Country {
            get {
                return this.country;
            }
            set {
                this.country = value;
            }
        }
    }
}
