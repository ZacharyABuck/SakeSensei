using Codenutz.XF.InfiniteListView.Shared.Core;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SakeSensei.Models
{
    [Table("Products")]
    public class Product : ObservableObject
    {
        public Product()
        {
        }

        [PrimaryKey, Column("ID")]
        public string ID { get; set; }

        [Column("SakeName")]
        public string SakeName { get; set; }

        [Column("Brewery")]
        public string Brewery { get; set; }

        [Column("Classification")]
        public string Classification { get; set; }

        [Column("Region")]
        public string Region { get; set; }

        [Column("SakeMeterValue")]
        public string SakeMeterValue { get; set; }

        [Column("Alcohol")]
        public string Alcohol { get; set; }

        [Column("Acidity")]
        public string Acidity { get; set; }

        [Column("PolishingRate")]
        public string PolishingRate { get; set; }

        [Column("VarietyOfRice")]
        public string VarietyOfRice { get; set; }

        [Column("ServingRecommendation")]
        public string ServingRecommendation { get; set; }

        [Column("FoodPairing")]
        public string FoodPairing { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("ImgL")]
        public string ImgL { get; set; }

        [Column("ImgM")]
        public string ImgM { get; set; }

        [Column("ImgS")]
        public string ImgS { get; set; }

        [Column("URLReference")]
        public string URLReference { get; set; }

        [Column("UPC")]
        public string UPC { get; set; }

        public override string ToString()
        {
            return SakeName;
        }
    }

    public class SakeDB
    {
        public static bool overwriteDB = true;

        private SQLiteAsyncConnection dbConn;

        public string StatusMessage { get; set; }

        public SakeDB(ISQLitePlatform sqlitePlatform, string dbPath)
        {
            //initialize a new SQLiteConnection 
            if (dbConn == null)
            {
                dbConn = new SQLiteAsyncConnection(dbPath);
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            //return a list of people saved to the Person table in the database
            List<Product> productTask = await dbConn.Table<Product>().ToListAsync();
            return productTask;
        }

        public async Task<List<Product>> GetProducts(int start, int limit)
        {
            //Compose the statement
            String statement = "SELECT * FROM Products ORDER BY SakeName LIMIT '" + start + "', " + limit;
            //Execute the query
            List<Product> moreProducts = await dbConn.QueryAsync<Product>(statement);
            return moreProducts;
        }
    }
}