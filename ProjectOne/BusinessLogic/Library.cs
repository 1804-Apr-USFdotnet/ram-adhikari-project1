using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /* all business logic must be here - sorting, search logic
     * Functionality
     * Display top 3 restaurants by average rating
     * Display all restaurants
     * Should allow more than one method of sorting
     * Display details of a restaurant
     * Display all the reviews of a restaurant
     * Search restaurants (e.g. by partial name), and display all matching results
     * Quit application*/
    public class Library
    {
        private DataLayer dataCrud = new DataLayer();

        //Add a restaurant
        public void Add(List<string> list)
        {
            dataCrud.AddRestaurant(list);
        }

        //Delete a restaurant
        public void Delete(int restaurantId)
        {
            dataCrud.DeleteRestaurant(restaurantId);            
        }


        //Modify a restaurant
        public void Modify(List<string> list)
        {
            dataCrud.ModifyRestaurant(list);
        }

        
        //Display one instance of a restaurant
        public void DisplayRestaurantById(int restaurantId)
        {
            Restaurant restaurant = dataCrud.FindRestaurantById(restaurantId);
            PrintRestaurant(restaurant);
        }

        //crud.PrintRestaurantsId();
        public void PrintIds()
        {
            PrintRestaurantsId();
        }
        private void PrintRestaurantsId()
        {
            List<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            Console.Write("Valid id's of restaurants: ");
            foreach (var res in restaurants)
            {
                Console.Write(" " + res.RestaurantId + "  ");
            }
            Console.WriteLine();
        }

        public int MinId()
        {
            List<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            int count = restaurants.Count;
            if (count >= 1)
            {
                return restaurants.ElementAt(0).RestaurantId;
            }
            else
            {
                return 0;
            }
            
        }

        public int MaxId()
        {
            List<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            int count = restaurants.Count;
            if(count >= 1)
            {
                return restaurants.ElementAt(count - 1).RestaurantId;
            }
            else
            {
                return 0;
            }
            
        }


        // crud.SortAndDisplayById();
        public void DisplaySortedId()
        {
            SortAndDisplayById();
        }
        private void SortAndDisplayById()
        {
            List<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            List<Restaurant> sortedRestaurants = restaurants.OrderBy(r => r.RestaurantId).ToList();
            foreach (var res in sortedRestaurants)
            {
                PrintRestaurant(res);
            }
        }


        //Sort by name and dispaly it 
        public void DisplaySortedName()
        {
            SortAndDisplayByName();
        }
        private void SortAndDisplayByName()
        {
            List<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            List<Restaurant> sortedRestaurants = restaurants.OrderBy(r => r.Name).ToList();
            foreach (var res in sortedRestaurants)
            {
                PrintRestaurant(res);
            }
        }


        //crud.DisplyAllReviewsOfRestaurant(i);
        public void DisplayReviewsOfRestaurant(int restaurantId)
        {
            List<Review> reviews = dataCrud.GetReviewsList();
            foreach (var rev in reviews)
            {
                if (rev.RestaurantId == restaurantId)
                {
                    PrintReview(rev);
                }
            }
        }
        private void PrintReview(Review rev)
        {
            Console.WriteLine("\nReview Id:     " + rev.ReviewId +
                              "\nRestaurant Id: " + rev.RestaurantId);
            Console.WriteLine("Rating:        {0:N2}", rev.Rating / 2.0);
            Console.WriteLine("Username:      " + rev.Username +
                              "\nComment:       " + rev.Comment +
                              "\nDate:          " + rev.ReviewDate);
        }


        //Display top 3 restaurants by average rating
        public void TopThree()
        {
            List<KeyValuePair<Restaurant,double>> avgRatingsList = TopThreeRated(false);
            int r = 0;
            foreach (var rating in avgRatingsList)
            {
                Console.Write("Restaurant Name: " + rating.Key.Name);
                Console.WriteLine("\nAverage Rating: {0:N2}\n", rating.Value / 2.0);
                r++;
                if (r == 3)
                {
                    break;
                }
            }
        }

        //Uses optional parameters 
        public List<KeyValuePair<Restaurant, double>> TopThreeRated(bool testing, List<Restaurant> resta = null,
            List<Review> rev = null)
        {
            List<Restaurant> restaurants = resta;
            List<Review> reviews = rev;
            //For unit testing purposes only 
            if (testing == false)
            {
                restaurants = dataCrud.GetRestaurantsList();
                reviews = dataCrud.GetReviewsList();
            }

            int[] sum = new int[restaurants.Count()];
            int restaCount = sum.Length;
            int reviewsCount = reviews.Count();

            for (int i = 0; i < restaCount; i++)
            {
                int restaurantId = restaurants.ElementAt(i).RestaurantId;
                for (int j = 0; j < reviewsCount; j++)
                {
                    int reviewsId = reviews.ElementAt(j).RestaurantId;
                    if (restaurantId.Equals(reviewsId))
                    {
                        int addRating = reviews.ElementAt(j).Rating;
                        sum[i] += addRating;
                    }
                }
            }
            Dictionary<Restaurant, double> dict = new Dictionary<Restaurant, double>();
            for (int k = 0; k < restaCount; k++)
            {
                Restaurant res = restaurants.ElementAt(k);
                double average = (double)sum[k] / TotalReviewsByResId(testing, res.RestaurantId, reviews);
                dict.Add(res, average);
            }
            
            var avgRatingsList = dict.ToList();
            avgRatingsList.Sort((rating1, rating2) => rating2.Value.CompareTo(rating1.Value));

            return avgRatingsList;
        }
        
        private int TotalReviewsByResId(bool testing, int restaurantId, List<Review> revs)
        {
            List<Review> reviews = revs;
            if(testing == false)
            {
                reviews = dataCrud.GetReviewsList();
            }
            int count = 0;
            foreach (var rev in reviews)
            {
                if (rev.RestaurantId == restaurantId)
                {
                    count++;
                }
            }
            return count;
        }

        //Partial search by name
        public void PartialSearch(string partialName)
        {
            List<Restaurant> restaurants = SearchedByPartialName(false, partialName);
            
            foreach (var res in restaurants)
            {
                PrintRestaurant(res);

            }
        }
        public List<Restaurant> SearchedByPartialName(bool testing, string partialName, List<Restaurant> resta = null)
        {
            List<Restaurant> restaurants = resta;
            List<Restaurant> returnList = new List<Restaurant>();
            if (testing == false)
            {
                restaurants = dataCrud.GetRestaurantsList();
            }
            foreach (var res in restaurants)
            {
                string resName = res.Name.ToLower();
                string partial = partialName.ToLower();
                if (partialName.Length > 0)
                {
                    if (resName.Contains(partial))
                    {
                        returnList.Add(res);
                    }
                }
            }
            return returnList;
        }
        private void PrintRestaurant(Restaurant res)
        {
            if(res != null)
            {
                Console.WriteLine("ID:          " + res.RestaurantId +
                  "\nName:      " + res.Name +
                  "\nAddress:   " + res.Address +
                  "\nPhone:     " + res.Phone +
                  "\nWebsite:   " + res.Website +
                  "\nDelivery:  " + res.DeliveryOption +
                  "\nFoodType:  " + res.FoodType + "\n");
            }
            else
            {
                Console.WriteLine("Restaurant not in the database.");
            }

        }



    }
}
