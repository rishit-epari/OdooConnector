using OdooConnector.Models;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
using System;
using System.Threading.Tasks;

namespace OdooConnector
{
   class Program
   {
      static async Task Main(string[] args)
      {
         await CheckConnection();
         //await GetModelSchema();
         await ReadData();
      }

      private static async Task CheckConnection()
      {
         OdooClient odooClient = OdooClientSetup();
         var versionResult = await odooClient.GetVersionAsync();

         if (versionResult.Succeed)
         {
            Console.WriteLine("Connection successful....");
            Console.WriteLine($"Server version : {versionResult.Value.ServerVersion}\n");

            var loginResult = await odooClient.LoginAsync();
            if (loginResult.Succeed)
            {
               Console.WriteLine("Login successful...");
               Console.WriteLine($"User id : {loginResult.Value}\n");
            }
            else
               Console.WriteLine("Login failed...");

         }
         else
            Console.WriteLine("Connection failure");
      }

      private static async Task GetModelSchema()
      {
         OdooClient odooClient = OdooClientSetup();

         var tableName = "mrp.production";
         var modelResult = await odooClient.GetModelAsync(tableName);

         var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
      }

      private static async Task ReadData()
      {
         var repository = new OdooRepository<ProductProductOdooModel>(OdooClientSetup().Config);
         var products = await repository.Query().ToListAsync();

         Console.WriteLine("Displaying product details...");
         foreach (var product in products.Value)
         {
            Console.WriteLine($"Active : {product.Active}");
            Console.WriteLine($"Description : {product.Description}");
            Console.WriteLine($"DisplayName : {product.DisplayName}");
            Console.WriteLine($"Product Id : {product.Id}");
            Console.WriteLine($"Quantity : {product.QtyAvailable}");
         }
      }

      private static OdooClient OdooClientSetup()
      {
         var config = new OdooConfig(
                             apiUrl: "https://central-innovation3.odoo.com", //  "http://localhost:8069"
                             dbName: "central-innovation3",
                             userName: "rishit.epari@centralinnovation.com",
                             password: "9778ac82852527e79faf0383389f20716ce6be0d"
                          );

         var odooClient = new OdooClient(config);
         return odooClient;
      }
   }
}
