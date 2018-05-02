using System;
using System.Collections.Generic;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;

namespace TestDriven
{
    /* Unit tests
    *  Should not cause actual database access
    *  Should develop some parts of application with test-driven development - writing the tests first
    *  
    *  Arrange - arranging the things that needs to be tested
    *  Assert - Compare against what you are expecting
    *  Assert.AreEqual(expectedFoodtype, actualFoodtype);
    */
    [TestClass]
    public class UnitTest
    {
        
        Restaurant res1 = new Restaurant() { RestaurantId = 1, Name = "Columbia", Address = "2117 E 7th Ave",
            Phone = "8132484961", Website = "columbiarestaurant.com", DeliveryOption = "No", FoodType = "Non-veg" };

        Restaurant res2 = new Restaurant(){ RestaurantId = 2, Name = "Loving Hut", Address = "1905 E Fletcher Ave",
            Phone = "8139777888", Website = "lovinghut.us/", DeliveryOption = "Yes", FoodType = "Vegan" };

        Restaurant res3 = new Restaurant() { RestaurantId = 3, Name = "SoFresh Salads & Grill", Address = "2774 E Fowler Ave",
            Phone = "8139774477", Website = "sofreshsalads.com", DeliveryOption = "Yes", FoodType = "All"};

        Restaurant res4 = new Restaurant() { RestaurantId = 4, Name = "Food", Address = "1234 Middle of Nowhere",
        Phone = "3248974765", Website = "what.com", DeliveryOption = "No", FoodType = "All"};
        
        Review rev1 = new Review() { ReviewId = 1, RestaurantId = 1, Rating = 9, Username = "Her", Comment = "Non-Veg" };

        Review rev2 = new Review(){ReviewId = 2, RestaurantId = 2, Rating = 8, Username = "Who", Comment = "Vegan" };

        Review rev3 = new Review(){ ReviewId = 3, RestaurantId = 3, Rating = 7, Username = "My", Comment = "Veg" };

        Review rev4 = new Review() { ReviewId = 4, RestaurantId = 4, Rating = 10, Username = "Them", Comment = "Where is this restaurant?" };

        

        [TestMethod]
        public void TestSerializer()
        {
            HelperSerialize<Restaurant> serializeTest = new HelperSerialize<Restaurant>();
            string fileName = serializeTest.getFilePath();
            List<Restaurant> resList = new List<Restaurant>();
            resList.Add(res1);

            serializeTest.SerializeRestaurants(resList, fileName);
            List<Restaurant> resDeserialized = serializeTest.DeserializeRestaurants(fileName);
            Restaurant expected = resDeserialized[0];
            Restaurant actual = resList[0];

            Assert.AreEqual(expected.RestaurantId, actual.RestaurantId);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Phone, actual.Phone);
            Assert.AreEqual(expected.Website, actual.Website);
            Assert.AreEqual(expected.DeliveryOption, actual.DeliveryOption);
            Assert.AreEqual(expected.FoodType, actual.FoodType);

        }


        [TestMethod]
        public void TestTopThreeRated()
        {
            List<Restaurant> restaList = new List<Restaurant>();
            List<Review> revsList = new List<Review>();
            restaList.Add(res1);
            restaList.Add(res2);
            restaList.Add(res3);
            restaList.Add(res4);

            revsList.Add(rev1);
            revsList.Add(rev2);
            revsList.Add(rev3);
            revsList.Add(rev4);

            Library libLogic = new Library();
            List<KeyValuePair<Restaurant, double>> avgRatings = libLogic.TopThreeRated(true,restaList, revsList);
            double expected1 = 10;
            double expected2 = 9;
            double expected3 = 8;

            double actaul1 = avgRatings[0].Value;
            double actual2 = avgRatings[1].Value;
            double actaul3 = avgRatings[2].Value;

            Assert.AreEqual(expected1, actaul1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actaul3);            
        }

        [TestMethod]
        public void TestPartialSearch()
        {
            List<Restaurant> restaurantList = new List<Restaurant>();
            restaurantList.Add(res1);
            restaurantList.Add(res2);
            restaurantList.Add(res3);
            restaurantList.Add(res4);

            Library libraryLogic = new Library();
            List<Restaurant> list = libraryLogic.SearchedByPartialName(true, "Col", restaurantList);

            string expected1 = "Columbia";
            string actual1 = list[0].Name;
            Assert.AreEqual(expected1, actual1);

            list = libraryLogic.SearchedByPartialName(true, "hut", restaurantList);
            string actual2 = list[0].Name;
            Assert.AreEqual("Loving Hut", actual2);
        }
    }
}
