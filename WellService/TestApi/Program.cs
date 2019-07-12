using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestApi
{
    class Program
    {
        const string URLAuth = "https://localhost:44333/Users/authenticate";
        const string GetClientUrl = "https://localhost:44333/api/ClientQuotas/GetQuotaByClientName?ClientName=";
        const string UpdateQuotaUrl = "https://localhost:44333/api/ClientQuotas/UpdateClientQuota";
        const string AddUserUrl = "https://localhost:44333/api/ClientQuotas/AddOrUpdateClient";
        const string GetAllClientUrl = "https://localhost:44333/api/ClientQuotas/GetAllClients";
        static async Task Main(string[] args)
        {
            var Token = "";
            var client = new HttpClient();
            //get token bearer with api key
            Console.WriteLine("--get token from api--");
            var resp = await client.PostAsync(URLAuth, new StringContent("{\"apiKey\": \"123qweasd\"}",Encoding.UTF8,"application/json"));
            if (resp.IsSuccessStatusCode)
            {
                Token = JsonConvert.DeserializeObject<User>(await resp.Content.ReadAsStringAsync()).Token;
                Console.WriteLine($"Token from api : {Token}");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }
            if (!string.IsNullOrEmpty(Token))
            {
                //get user by name
                Console.WriteLine("--Get user : client 1--");
                resp = await client.GetAsync(GetClientUrl + "client 1");
                if (resp.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<OutputData>(await resp.Content.ReadAsStringAsync());
                    var users = ((JArray)data.Data).ToObject<List<ClientQuota>>();

                    foreach (var usr in users)
                    {
                        Console.WriteLine($"user: {usr.ClientName} - quota: {usr.Quota}");
                    }
                }

                //update quota
                Console.WriteLine("--update quota : clientid - 1, quota - 11--");
                resp = await client.PostAsync(UpdateQuotaUrl, new StringContent( JsonConvert.SerializeObject(new ClientQuota() { ClientID=1, Quota=11, ClientName="asep@oke.com", Email="", UpdatedDate=DateTime.Now })  , Encoding.UTF8, "application/json"));
                if (resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("client with id 1 is updated with 11 quota");
                }
                else
                {
                    Console.WriteLine("fail to update quota");
                }

                //add more client
                var rnd = new Random();
                Console.WriteLine("--add user : name : asep n, quota - 22--");
                resp = await client.PostAsync(AddUserUrl, new StringContent(JsonConvert.SerializeObject(new ClientQuota() { Email="asep@oke.com", ClientName="Asep "+rnd.Next(1000), Quota = 22 }), Encoding.UTF8, "application/json"));
                if (resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("add client succeed");
                }
                else
                {
                    Console.WriteLine("fail to add client");
                }

                //get all client
                
                Console.WriteLine("--Get all users--");
                resp = await client.GetAsync(GetAllClientUrl );
                if (resp.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<OutputData>(await resp.Content.ReadAsStringAsync());
                    var users = ((JArray)data.Data).ToObject<List<ClientQuota>>();
                    foreach (var usr in users)
                    {
                        Console.WriteLine($"user: {usr.ClientName} - quota: {usr.Quota}");
                    }
                }
            }
            Console.WriteLine("finished.");
            Console.ReadLine();
        }
       
    }
    public class OutputData
    {
        public bool IsSucceed { set; get; }
        public object Data { set; get; }
        public string ErrorMessage { set; get; }
    }
    public class ClientQuota
    {
        public string Email { get; set; }
        public int Quota { get; set; }
        
        public int ClientID { get; set; }
       
        public string ClientName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
