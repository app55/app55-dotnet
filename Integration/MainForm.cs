using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Integration {

    internal class WriteMessageEventArgs : EventArgs {
        private string message;

        public WriteMessageEventArgs(string message) {
            this.message = message;
        }

        public string Message {
            get {
                return message;
            }
        }
    }

    public partial class MainForm : Form {
        private App55.Gateway gateway;
        private Thread thread;

        public MainForm() {
            InitializeComponent();
            Stop += new EventHandler<EventArgs>(MainForm_OnStop);
            WriteMessage += new EventHandler<WriteMessageEventArgs>(MainForm_OnWrite);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.Save();
        }

        private App55.UserCreateResponse CreateUser(string email, string phone, string password, string confirmPassword) {
            Write("Creating user " + email + "...");
            App55.UserCreateResponse response = gateway.CreateUser(new App55.User(email, phone, password, confirmPassword)).Send();
            WriteLn(" DONE (user-id " + response.User.ID + ")");
            return response;
        }

        private App55.CardCreateResponse CreateCard(App55.User user) {
            Write("Creating card...");
            App55.CardCreateResponse response = gateway.CreateCard(new App55.User((long)user.ID), new App55.Card(
                "App55 User", "4111111111111111", DateTime.UtcNow.AddDays(90).ToString("MM/yyyy"), "111", null,
                new App55.Address(
                    "8 Exchange Quay", "Manchester", "M5 3EJ", "GB"
                )
            )).Send();
            WriteLn(" DONE (token " + response.Card.Token + ")");
            return response;
        }

        private App55.CardListResponse ListCards(App55.User user) {
            Write("Listing cards...");
            App55.CardListResponse response = gateway.ListCards(new App55.User((long)user.ID)).Send();
            WriteLn(" DONE (" + response.Cards.Count + " cards)");
            return response;
        }

        private App55.CardDeleteResponse DeleteCard(App55.User user, App55.Card card) {
            Write("Deleting card " + card.Token + "...");
            App55.CardDeleteResponse response = gateway.DeleteCard(new App55.User((long)user.ID), new App55.Card(card.Token)).Send();
            WriteLn(" DONE");
            return response;
        }

        private App55.TransactionCreateResponse CreateTransaction(App55.User user, App55.Card card) {
            Write("Creating transaction...");
            App55.TransactionCreateResponse response = gateway.CreateTransaction(new App55.User((long)user.ID), new App55.Card(card.Token),
                new App55.Transaction("0.10", "GBP", null)
            ).Send();

            WriteLn(" DONE (transaction-id " + response.Transaction.ID + ")");
            return response;
        }

        private App55.TransactionCommitResponse CommitTransaction(App55.Transaction transaction) {
            Write("Committing transaction...");
            App55.TransactionCommitResponse response = gateway.CommitTransaction(new App55.Transaction(transaction.ID)).Send();
            WriteLn(" DONE");
            return response;
        }
        

        private void Start() {
            try {
                WriteLn("");
                WriteLn("App55 Sandbox - API Key <" + gateway.ApiKey + ">");
                WriteLn("");

                App55.User user = CreateUser("example." + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "@app55.com", "0123 456 7890", "pa55word", "pa55word").User;

                App55.Card card1 = CreateCard(user).Card;
                App55.Transaction transaction = CreateTransaction(user, card1).Transaction;
                CommitTransaction(transaction);

                App55.Card card2 = CreateCard(user).Card;
                transaction = CreateTransaction(user, card2).Transaction;
                CommitTransaction(transaction);

                App55.Card card3 = CreateCard(user).Card;
                transaction = CreateTransaction(user, card3).Transaction;
                CommitTransaction(transaction);

                IList<App55.Card> cards = ListCards(user).Cards;
                if(cards.Count != 3) throw new Exception("ListCards should contain 3 cards");
                if(!cards.Contains(card1)) throw new Exception("ListCards is missing card " + card1.Token);
                if(!cards.Contains(card2)) throw new Exception("ListCards is missing card " + card2.Token);
                if(!cards.Contains(card3)) throw new Exception("ListCards is missing card " + card3.Token);

                foreach(App55.Card card in cards) {
                    DeleteCard(user, card);
                }

                if(ListCards(user).Cards.Count != 0) throw new Exception("ListCards should contain 0 cards");

                string email = "example." + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "@app55.com";
                Write("Updating user...");
                gateway.UpdateUser(new App55.User((long)user.ID, email, "password01", "password01")).Send();
                WriteLn(" DONE");

                Write("Authenticating user...");
                App55.User user2 = gateway.AuthenticateUser(new App55.User(email, "password01")).Send().User;
                WriteLn(" DONE");

                if(user.ID != user2.ID) throw new Exception("User ID mismatch after update user");

                WriteLn("");
                WriteLn("Successful");
                WriteLn("===========================");
                WriteLn("");

            } catch(Exception e) {
                WriteLn(e.ToString());
                WriteLn(e.StackTrace);
            } finally {
                RaiseStop();
            }
        }

        

        private void startStopButton_Click(object sender, EventArgs e) {
            if(thread == null) {
                startStopButton.Text = "Stop";
                gateway = new App55.Gateway(App55.Environment.Sandbox, Properties.Settings.Default.apiKey, Properties.Settings.Default.apiSecret);
                thread = new Thread(Start);
                thread.Start();
            } else {
                thread.Abort();
                RaiseStop();
            }
        }

        private static event EventHandler<EventArgs> Stop;
        private static void RaiseStop() {
            if(Stop != null) Stop(null, EventArgs.Empty);
        }

        private delegate void MainForm_OnStopCallback(object sender, EventArgs e);
        private void MainForm_OnStop(object sender, EventArgs e) {
            if(this.startStopButton.InvokeRequired) {
                MainForm_OnStopCallback d = new MainForm_OnStopCallback(MainForm_OnStop);
                this.Invoke(d, new object[] { sender, e });
            } else {
                thread = null;
                startStopButton.Text = "Start";
                gateway = null;
            }
        }


        private void Write(string message) {
            RaiseWriteMessage(message);
        }

        private void WriteLn(string message) {
            Write(message);
            Write("\r\n");
        }

        private delegate void MainForm_OnWriteCallback(object sender, WriteMessageEventArgs e);
        private void MainForm_OnWrite(object sender, WriteMessageEventArgs e) {
            if(this.outputTextBox.InvokeRequired) {
                MainForm_OnWriteCallback d = new MainForm_OnWriteCallback(MainForm_OnWrite);
                this.Invoke(d, new object[] { sender, e });
            } else {
                this.outputTextBox.Text += e.Message;
                this.outputTextBox.SelectionStart = this.outputTextBox.Text.Length;
                this.outputTextBox.ScrollToCaret();
            }
        }

        private static event EventHandler<WriteMessageEventArgs> WriteMessage;
        private static void RaiseWriteMessage(string message) {
            if(WriteMessage != null) WriteMessage(null, new WriteMessageEventArgs(message));
        }
    }
}
