﻿using RentDesktop.Models;
using RentDesktop.Models.DB;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace RentDesktop.Infrastructure.Services.DB
{
    internal static class ShopService
    {
        public static List<Transport> GetTransports()
        {
            //transports = new[]
            //{
            //    new Transport("1", "Lada 7", "Company 1", 10000, DateTime.Now, new Bitmap(@"D:\Testing\TechRental\lada7.jpg")),
            //    new Transport("2", "Lada 10", "Company 1", 5000, DateTime.Now, new Bitmap(@"D:\Testing\TechRental\lada10.jpg")),
            //    new Transport("3", "Lada 15", "Company 1", 7000, DateTime.Now, new Bitmap(@"D:\Testing\TechRental\lada15.jpg")),
            //    new Transport("4", "Niva", "Company 2", 25000, DateTime.Now, new Bitmap(@"D:\Testing\TechRental\niva.jpg")),
            //    new Transport("5", "UAZ", "Company 3", 30000, DateTime.Now, new Bitmap(@"D:\Testing\TechRental\uaz.jpg")),
            //};

            List<Transport> transports = new List<Transport>();

            using var db = new DatabaseConnectionService();

            int currentPage = 1;
            IEnumerable<DbOrder> currentOrder;

            do
            {
                string getOrdersHandle = $"/api/Order?page={currentPage++}";
                using HttpResponseMessage getOrdersResponse = db.GetAsync(getOrdersHandle).Result;

                if (!getOrdersResponse.IsSuccessStatusCode)
                    throw new ErrorResponseException(getOrdersResponse);

                var orderCollection = getOrdersResponse.Content.ReadFromJsonAsync<DbOrderCollection>().Result;

                if (orderCollection is null || orderCollection.orders is null)
                    throw new IncorrectContentException(getOrdersResponse.Content);

                var transportsCollection = DatabaseModelConverterService.ConvertOrders(orderCollection.orders)
                    .Select(t => t.Models);

                foreach (var currTransports in transportsCollection)
                    transports.AddRange(currTransports);

                currentOrder = orderCollection.orders;
            }
            while (currentOrder.Any());

            return transports;
        }
    }
}
