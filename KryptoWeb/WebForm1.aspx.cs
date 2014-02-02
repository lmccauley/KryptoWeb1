using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;
using System.Net;
using System.Xml.Linq;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Reflection;


namespace KryptoWeb
{
   
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (WebClient wb = new WebClient())
            {
                string url = "http://api.multipool.us/api.php?api_key=37c4af0205ca9fd29c7a0599065ee2aab42f8c9721c2855c3bb862429c482942";
                string json = wb.DownloadString(url);                

                var cur = JObject.Parse(json).GetValue("currency").ToObject<JObject>();                
                //var c = obj.GetValue("currency");

                //JObject cur = c.ToObject<JObject>();

                List<Coin> coins = new List<Coin>();

                foreach (JToken child in cur.Children())
                {                    
                    JsonReader reader = child.CreateReader();
                    int i = 0;
                    object coin = null;
                    object balance = null;

                    while (reader.Read())
                    {
                        var val = reader.Value;

                        if (i == 0)
                            coin = val;

                        // value is next record
                        if (val != null && val.ToString() == Common.CONFIRMED_REWARDS)
                        {
                            reader.Read();
                            balance = reader.Value;
                            coins.Add(new Coin(coin.ToString().ToUpper(), balance.ToString()));
                            break;
                        }
                        i++;
                        
                    }
                }

                foreach (Coin coin in coins)
                {
                    DataLayer.InsertPoolInfo(coin);
                }

                gvSummary.DataSource = DataLayer.GetSummary();
                gvSummary.DataBind();

                //foreach (var x in cur)
                //{
                //    string name = x.Key;
                //    JToken value = x.Value;                    
                //}

                //Currency currency = c.ToObject<Currency>();

                //Type type = currency.GetType();
                //PropertyInfo[] properties = type.GetProperties();

                //foreach (PropertyInfo property in properties)
                //{
                //    string coin = property.PropertyType.FullName;
                //    // dynamically instantiate object based on property name to get coin properties
                //    var c1 = Activator.CreateInstance(null, coin);                    
                //    //Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(type, null));
                //}            
                

            }

 
        }
    }

    public class Coin
    {
        public string coinName { get; set; }
        public string balance { get; set; }

        public Coin() { }

        public Coin(string c, string b)
        {
            coinName = c;
            balance = b;
        }
    }
}