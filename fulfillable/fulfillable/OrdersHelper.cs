using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace fulfillable
{
    class OrdersHelper
    {

        public static Tuple<string ,List<OrdersData>> Get_orders(string json_str)
        {
            string errorMsg = string.Empty;
            try
            {
                Dictionary<string, int> params_ok = CheckParameter(json_str.Trim(' '));
                if (params_ok != null)
                {
                    return new Tuple<string, List<OrdersData>>(errorMsg, Getlist(params_ok));
                }
                else errorMsg= ("Invalid input parameters!") ;
            }
            catch (FileNotFoundException e) { errorMsg = e.Message; }
            catch (FormatException e) { errorMsg = string.Concat("csv fileError: ", e.Message); }
            return new Tuple<string, List<OrdersData>>(errorMsg, null); 
        }
        
        private static Dictionary<string, int> CheckParameter(string json_str)
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            try
            {
                if (!string.IsNullOrEmpty(json_str))
                {
                    var paramTree = JsonConvert.DeserializeObject<JObject>(json_str);
                    foreach (var item in paramTree.Properties())
                    {
                        ret.Add(item.Name, Int32.Parse(item.Value.ToObject<object>().ToString()));
                    }
                } else ret = null;            
            }
            catch { ret = null; }

            return ret;
        }

        public  class OrdersData
        {
            public string product_id { get; set; }
            public int quantity { get; set; }
            public int priority { get; set; }
            public string priorityName { get; set; }
            public DateTime created_at { get; set; }

        }

        
        private static List<OrdersData> Getlist(Dictionary<string, int> orders)
        {
            List<OrdersData> ordesListd_sort = new List<OrdersData>();
            string Setpriority(int prior)
            {
                string ret = string.Empty;
                switch (prior)
                {
                    case 1: ret = "low"; break;
                    case 2: ret = "medium"; break; 
                    case 3: ret = "high"; break;
                    default: ret = "no data"; break;
                }
                return ret;
            }

            try
            {
                using (var reader = new StreamReader(@"orders.csv"))
                {

                    List<OrdersData> ordesList = new List<OrdersData>();
                    int counter = 1;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (counter > 1)
                        {
                            var values = line.Split(',');
                            try
                            {
                                if (orders.Any(p => p.Key == values[0] && p.Value >= int.Parse(values[1])))
                                {
                                    OrdersData item = new OrdersData();
                                    item.product_id = values[0];
                                    item.quantity = int.Parse(values[1]);
                                    item.priority = int.Parse(values[2]);
                                    item.priorityName = Setpriority(int.Parse(values[2]));
                                    item.created_at = DateTime.Parse(values[3]);
                                    ordesList.Add(item);
                                }
                            }
                            finally {}
                        }
                        counter++;
                    }
                    ordesListd_sort = ordesList.OrderBy(x => x.created_at).OrderByDescending(x => x.priority).ToList();
                }
            }
            
            catch (FormatException e) { throw; }
            return ordesListd_sort;
        }
    }
}
