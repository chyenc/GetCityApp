using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace GetCityApp
{
    class Program
    {
        static string MainUrl = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2020/";

        static DataTable dt = new DataTable();
        static DataTable Getcitytr_dt = new DataTable();
        static DataTable Getcountytr_dt = new DataTable();
        static DataTable Gettowntr_dt = new DataTable();
        static DataTable Getvillagetr_dt = new DataTable();
        static int count = 0;

        static CookieContainer cc = new CookieContainer();

        static List<string> ports = new List<string>();

        static string html = string.Empty;

        static void Main(string[] args)
        {

            DoCommand("DELETE FROM kangyu_all_city_list");

            //ports.Add("59.38.62.109,9797");
            //ports.Add("112.253.11.113,8000");
            //ports.Add("222.189.191.206,9999");
            //ports.Add("118.212.104.138,9999");
            //ports.Add("121.232.194.25,9000");
            //ports.Add("115.218.2.139,9000");
            //ports.Add("49.89.84.107,9999");
            //ports.Add("171.12.220.36,9999");
            //ports.Add("175.44.108.155,9999");
            //ports.Add("182.32.163.128,9999");
            //ports.Add("112.84.98.235,9999");
            //ports.Add("1.197.203.59,9999");
            //ports.Add("175.42.122.46,9999");
            //ports.Add("163.204.240.134,9999");
            //ports.Add("19.254.94.114,34422");


            string page = "index.html";

            //GetPageHtml(MainUrl + page);
            var htmlpage = WebHelper.WebFormGet(MainUrl + page, "");
            // var maincity = GetMainCity(htmlpage);
            Dictionary<string, string> MainCity = GetMainCity(htmlpage);

            //DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("P_ID");
            dt.Columns.Add("NAME");
            dt.Columns.Add("URL");
            dt.Columns.Add("LEVEL_ID");

            Getcitytr_dt.Columns.Add("ID");
            Getcitytr_dt.Columns.Add("P_ID");
            Getcitytr_dt.Columns.Add("NAME");
            Getcitytr_dt.Columns.Add("URL");
            Getcitytr_dt.Columns.Add("LEVEL_ID");

            Getcountytr_dt.Columns.Add("ID");
            Getcountytr_dt.Columns.Add("P_ID");
            Getcountytr_dt.Columns.Add("NAME");
            Getcountytr_dt.Columns.Add("URL");
            Getcountytr_dt.Columns.Add("LEVEL_ID");

            Gettowntr_dt.Columns.Add("ID");
            Gettowntr_dt.Columns.Add("P_ID");
            Gettowntr_dt.Columns.Add("NAME");
            Gettowntr_dt.Columns.Add("URL");
            Gettowntr_dt.Columns.Add("LEVEL_ID");


            Getvillagetr_dt.Columns.Add("ID");
            Getvillagetr_dt.Columns.Add("P_ID");
            Getvillagetr_dt.Columns.Add("NAME");
            Getvillagetr_dt.Columns.Add("URL");
            Getvillagetr_dt.Columns.Add("LEVEL_ID");






            foreach (KeyValuePair<string, string> item in MainCity)
            {
                TableAddRow(dt, item.Key, "", item.Key, MainUrl + item.Value, 1);
                //Console.WriteLine("ADD:" + item.Key + "——" + item.Value);
            }


            foreach (DataRow dr in dt.Rows)
            {

                Getcitytr(dr["url"].ToString(), dr["id"].ToString());
                //Thread.Sleep(1000);
            }

            foreach (DataRow dr in Getcitytr_dt.Rows)
            {
                Getcountytr(dr["url"].ToString(), dr["id"].ToString());
                //Thread.Sleep(1000);
            }


            foreach (DataRow dr in Getcountytr_dt.Rows)
            {
                if (dr["url"].ToString() != "")
                {
                    Gettowntr(dr["url"].ToString(), dr["id"].ToString());
                }
                //Thread.Sleep(1000);
            }


            //foreach (DataRow dr in Gettowntr_dt.Rows)
            //{
            //    if (dr["url"].ToString() != "")
            //    {
            //        Getvillagetr(dr["url"].ToString(), dr["id"].ToString());
            //    }
            //    //Thread.Sleep(1000);
            //}

        }

        private static void GetPageHtml(string Url)
        {
            html = WebHelper.WebFormGet(Url, "");
        }






        /// <summary>
        /// 第一级
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetMainCity(string Html)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string html = Html.Substring(Html.IndexOf("<tr class='provincetr'>"));
            string keyValue = "<td><a href='";
            while (html.Contains(keyValue))
            {
                html = html.Substring(html.IndexOf(keyValue) + keyValue.Length);
                string url = html.Substring(0, html.IndexOf("'"));
                string name = html.Substring(html.IndexOf(url) + url.Length + 2);
                html = name;
                name = name.Substring(0, name.IndexOf("<br/>"));

                dic.Add(name, url);


            }

            return dic;


        }

        /// <summary>
        /// 第二级
        /// </summary>
        /// <param name="url"></param>
        private static void Getcitytr(string url, string p_id)
        {
            GetPageHtml(url);
            string key = "<tr class='citytr'>";
            List<string> cityList = new List<string>();
            html = html.Substring(html.IndexOf(key));
            html = html.Substring(0, html.IndexOf("</table"));
            string p_url = url.Substring(0, url.LastIndexOf("/") + 1);
            while (html.Contains(key))
            {
                html = html.Substring(html.IndexOf(key));
                string cityname = html.Substring(0, html.IndexOf("</tr>"));
                cityList.Add(cityname);

                html = html.Substring(html.IndexOf(cityname) + cityname.Length);
            }


            foreach (string city in cityList)
            {
                string cityurl = city.Substring(city.IndexOf("<a href='") + 9);
                cityurl = cityurl.Substring(0, cityurl.IndexOf("'"));


                string id = city.Substring(city.IndexOf(cityurl) + cityurl.Length + 2);
                id = id.Substring(0, id.IndexOf("<"));

                string cityname = city.Substring(city.IndexOf("<a href='") + 9);
                cityname = city.Substring(city.LastIndexOf("<td>"));
                cityname = cityname.Replace("<td>", "").Replace("</td>", "").Replace("<a href='" + cityurl + "'>", "").Replace("</a>", "");
                if (cityname == "县")
                {

                }
                //cityname = cityname.Substring(cityname.IndexOf(cityurl) + cityurl.Length + 2);


                cityurl = p_url + cityurl;

                TableAddRow(Getcitytr_dt, id, p_id, cityname, cityurl, 2);
            }

        }



        /// <summary>
        /// 第三级
        /// </summary>
        /// <param name="url"></param>
        private static void Getcountytr(string url, string p_id)
        {

            GetPageHtml(url);

            string key = "<tr class='countytr'>";
            List<string> cityList = new List<string>();
            if (html.IndexOf(key) == -1)
            {
                key = "<tr class='towntr'>";
            }
            html = html.Substring(html.IndexOf(key));

            html = html.Substring(0, html.IndexOf("</table"));
            string p_url = url.Substring(0, url.LastIndexOf("/") + 1);
            while (html.Contains(key))
            {
                html = html.Substring(html.IndexOf(key));
                string cityname = html.Substring(0, html.IndexOf("</tr>"));
                cityList.Add(cityname);

                html = html.Substring(html.IndexOf(cityname) + cityname.Length);
            }


            foreach (string city in cityList)
            {
                string cityurl = city.Substring(city.IndexOf("<a href='") + 9);
                cityurl = cityurl.Substring(0, cityurl.IndexOf("'"));
                string id = string.Empty;

                if (city.Contains("市辖区"))
                {

                }
                id = city.Substring(city.IndexOf(cityurl) + cityurl.Length + 2);
                id = id.Substring(0, id.IndexOf("<"));
                if (!city.Contains("<a"))
                {
                    cityurl = "";
                    id = city.Substring(city.IndexOf("<td>") + 4);
                    id = id.Substring(0, id.IndexOf("</td>"));
                }


                string cityname = city.Substring(city.IndexOf("<a href='") + 9);
                cityname = city.Substring(city.LastIndexOf("<td>"));
                cityname = cityname.Replace("<td>", "").Replace("</td>", "").Replace("<a href='" + cityurl + "'>", "").Replace("</a>", "");
                //cityname = cityname.Substring(cityname.IndexOf(cityurl) + cityurl.Length + 2);


                cityurl = cityurl.Contains(".html") ? p_url + cityurl : "";

                TableAddRow(Getcountytr_dt, id, p_id, cityname, cityurl, 3);
            }

        }

        /// <summary>
        /// 第四级
        /// </summary>
        /// <param name="url"></param>
        /// <param name="p_id"></param>
        private static void Gettowntr(string url, string p_id)
        {
            GetPageHtml(url);

            string key = "<tr class='towntr'>";
            List<string> cityList = new List<string>();
            var let = html.IndexOf(key);
            if (let > 0)
            {
                html = html.Substring(let);
                html = html.Substring(0, html.IndexOf("</table"));
                string p_url = url.Substring(0, url.LastIndexOf("/") + 1);
                while (html.Contains(key))
                {
                    html = html.Substring(html.IndexOf(key));
                    string cityname = html.Substring(0, html.IndexOf("</tr>"));
                    cityList.Add(cityname);

                    html = html.Substring(html.IndexOf(cityname) + cityname.Length);
                }


                foreach (string city in cityList)
                {
                    string cityurl = city.Substring(city.IndexOf("<a href='") + 9);
                    cityurl = cityurl.Substring(0, cityurl.IndexOf("'"));
                    string id = string.Empty;


                    id = city.Substring(city.IndexOf(cityurl) + cityurl.Length + 2);
                    id = id.Substring(0, id.IndexOf("<"));
                    if (!city.Contains("<a"))
                    {
                        cityurl = "";
                        id = city.Substring(city.IndexOf("<td>") + 4);
                        id = id.Substring(0, id.IndexOf("</td>"));
                    }


                    string cityname = city.Substring(city.IndexOf("<a href='") + 9);
                    cityname = city.Substring(city.LastIndexOf("<td>"));
                    cityname = cityname.Replace("<td>", "").Replace("</td>", "").Replace("<a href='" + cityurl + "'>", "").Replace("</a>", "");
                    //cityname = cityname.Substring(cityname.IndexOf(cityurl) + cityurl.Length + 2);


                    cityurl = cityurl.Contains(".html") ? p_url + cityurl : "";


                    TableAddRow(Gettowntr_dt, id, p_id, cityname, cityurl, 4);
                }

            }
        }

        /// <summary>
        /// 第五级
        /// </summary>
        /// <param name="url"></param>
        /// <param name="p_id"></param>
        private static void Getvillagetr(string url, string p_id)
        {
            GetPageHtml(url);

            string key = "<tr class='villagetr'>";
            List<string> cityList = new List<string>();
            html = html.Substring(html.IndexOf(key));
            html = html.Substring(0, html.IndexOf("</table"));
            string p_url = url.Substring(0, url.LastIndexOf("/") + 1);
            while (html.Contains(key))
            {
                html = html.Substring(html.IndexOf(key));
                string cityname = html.Substring(0, html.IndexOf("</tr>"));
                cityList.Add(cityname);

                html = html.Substring(html.IndexOf(cityname) + cityname.Length);
            }


            foreach (string city in cityList)
            {
                //city = "<tr class=\"villagetr\"><td>441900003001</td><td>111</td><td>岗贝社区居民委员会</td></tr>";
                string cityurl = "";

                string id = string.Empty;


                id = city.Substring(city.IndexOf("<td>") + 4);
                id = id.Substring(0, id.IndexOf("</td>"));



                string cityname = city.Substring(city.LastIndexOf("<td>") + 4);
                cityname = city.Substring(city.LastIndexOf("</td>"));
                //cityname = cityname.Replace("<td>", "").Replace("</td>", "").Replace("<a href='" + cityurl + "'>", "").Replace("</a>", "");
                //cityname = cityname.Substring(cityname.IndexOf(cityurl) + cityurl.Length + 2);


                //cityurl = p_url + cityurl;

                TableAddRow(Getvillagetr_dt, id, p_id, cityname, cityurl, 5);
            }
        }

        private static void TableAddRow(DataTable DT, string id, string p_id, string name, string url, int level_id)
        {
            count++;
            DataRow dr = DT.NewRow();
            dr["ID"] = id;
            dr["P_ID"] = p_id;
            dr["NAME"] = name;
            dr["URL"] = url;
            dr["LEVEL_ID"] = level_id.ToString();
            DT.Rows.Add(dr);
            InsetData(id, p_id, name, level_id);
            Console.WriteLine(count.ToString() + " 第" + level_id.ToString() + "级 " + id + "------" + name);
        }


        private static void InsetData(string id, string p_id, string name, int level_id)
        {
            DoCommand("INSERT INTO kangyu_all_city_list VALUES('" + id + "','" + p_id + "','" + name + "'," + level_id + ")");
        }
        public static void DoCommand(string ConnString)
        {

            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=city_db.mdb"); //Jet OLEDB:Database Password=
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();

                cmd.CommandText = ConnString;
                cmd.ExecuteNonQuery();
            }
            catch (Exception EX) { }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
