using JasenrekisteriOhjelmaNoSQL.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace JasenrekisteriOhjelmaNoSQL
{
    public partial class MainPage : ContentPage
    {
        public MongoClient client = new MongoClient("mongodb://localhost:27017");
        private string OldEmail = "";

        public MainPage()
        {
            InitializeComponent();
            GetData();
        }

        private async void GetData()
        {
            // Hae collection & sen data
            var collection = client.GetDatabase("JasenRekisteri").GetCollection<BsonDocument>("Jasenet");
            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = await collection.Find(filter).ToListAsync();

            // Muuta data
            var data = documents.Select(v => BsonSerializer.Deserialize<Jasenet>(v)).ToList();

            memberList.ItemsSource = data;
        }

        private void memberList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = (Jasenet)memberList.SelectedItem;

            eFirstName.Text = selected.FirstName;
            eLastName.Text = selected.LastName;
            eAddress.Text = selected.Address;
            ePostalCode.Text = selected.PostalCode;
            ePhoneNumber.Text = selected.Phone;
            eEmail.Text = selected.Email;
            eMembershipStartDt.Text = selected.MembershipStartDt;

            OldEmail = eEmail.Text;
        }

        private async void addButton_Clicked(object sender, EventArgs e)
        {
            var collection = client.GetDatabase("JasenRekisteri").GetCollection<BsonDocument>("Jasenet");

            var data = new BsonDocument
            {
                { "FirstName", aFirstName.Text },
                { "LastName", aLastName.Text },
                { "Address", aAddress.Text },
                { "PostalCode", aPostalCode.Text },
                { "Phone", aPhoneNumber.Text },
                { "Email", aEmail.Text },
                { "MembershipStartDt", aMembershipStartDt.Text }
            };

            // Lisää tietokantaan
            await collection.InsertOneAsync(data);

            // Nollaa input fieldid
            aFirstName.Text = "";
            aLastName.Text = "";
            aAddress.Text = "";
            aPostalCode.Text = "";
            aPhoneNumber.Text = "";
            aEmail.Text = "";
            aMembershipStartDt.Text = "";

            await DisplayAlert("Jäsen lisätty", "Jäsen lisätty onnistuneesti", "Ok");

            // Päivitä lista
            GetData();
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            // Hae collection
            var collection = client.GetDatabase("JasenRekisteri").GetCollection<BsonDocument>("Jasenet");

            // Hae muokattava entry
            var filter = Builders<BsonDocument>.Filter.Eq("Email", OldEmail);

            var update = Builders<BsonDocument>.Update.
                        Set("FirstName", eFirstName.Text).
                        Set("LastName", eLastName.Text).
                        Set("Address", eAddress.Text).
                        Set("PostalCode", ePostalCode.Text).
                        Set("Phone", ePhoneNumber.Text).
                        Set("Email", eEmail.Text).
                        Set("MembershipStartDt", eMembershipStartDt.Text);

            await collection.UpdateOneAsync(filter, update);

            // Nollaa input fieldid
            eFirstName.Text = "";
            eLastName.Text = "";
            eAddress.Text = "";
            ePostalCode.Text = "";
            ePhoneNumber.Text = "";
            eEmail.Text = "";
            eMembershipStartDt.Text = "";

            await DisplayAlert("Jäsentä muokattu", "Jäsentä muokattu onnistuneesti", "Ok");

            // Päivitä lista
            GetData();
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            var collection = client.GetDatabase("JasenRekisteri").GetCollection<BsonDocument>("Jasenet");

            var filter = Builders<BsonDocument>.Filter.Eq("Email", OldEmail);

            await collection.DeleteOneAsync(filter);

            GetData();
        }

    }

}
