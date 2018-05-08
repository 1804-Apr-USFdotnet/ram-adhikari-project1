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

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return dataCrud.GetRestaurantsList();
        }

        private IEnumerable<Restaurant> SortByDelivery()
        {
            IEnumerable<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            List<Restaurant> sortedRestaurants = restaurants.OrderBy(r => r.DeliveryOption).ToList();
            return sortedRestaurants;
        }

        private IEnumerable<Restaurant> SortByName()
        {
            IEnumerable<Restaurant> restaurants = dataCrud.GetRestaurantsList();
            List<Restaurant> sortedRestaurants = restaurants.OrderBy(r => r.Name).ToList();
            return sortedRestaurants;
        }


        //Return top three as a list
        public IEnumerable<String> TopThree()
        {
            List<String> top = new List<string>();
            List<KeyValuePair<Restaurant,double>> avgRatingsList = TopThreeRated(false);
            int r = 0;
            foreach (var rating in avgRatingsList)
            {
                top.Add(rating.Key.Name);
                top.Add((rating.Value / 2.0).ToString("0.##"));
                r++;
                if (r == 3)
                {
                    break;
                }
            }
            return top;
        }


        //Uses optional parameters 
        public List<KeyValuePair<Restaurant, double>> TopThreeRated(bool testing, List<Restaurant> resta = null,
            List<Review> rev = null)
        {
            IEnumerable<Restaurant> restaurants = resta;
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


        public IEnumerable<Restaurant> PartialNameSearch(bool testing, string partialName, List<Restaurant> testResta = null)
        {
            IEnumerable<Restaurant> rests = testResta;

            if(testing == false)
            {
                rests = dataCrud.GetRestaurantsList();
            }            

            if (!String.IsNullOrEmpty(partialName))
            {
                rests = rests.Where(r => r.Name.ToLower().Contains(partialName.ToLower()));
            }           
            return rests;
        }


        public Restaurant GetRestaurantById(int? id)
        {
            return dataCrud.GetRestaurantsList().Where(x => x.RestaurantId == id).FirstOrDefault();
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return dataCrud.GetReviewsList();
        }

        public Review GetReviewById(int? id)
        {
            return GetAllReviews().Where(x => x.ReviewId == id).FirstOrDefault();
        }
        
        public void AddReview(List<string> list)
        {
            dataCrud.AddReview(list);
        }

        public void ModifyReview(List<string> list)
        {
            dataCrud.ModifyReview(list);
        }

        public void DeleteReview(int reviewId)
        {
            dataCrud.DeleteReview(reviewId);
        }
    }
}
