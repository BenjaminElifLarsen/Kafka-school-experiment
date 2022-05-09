

/* Data handling
 * Avg use per house per day.
 * peak use per house per day
 * lowest use per house per day.
 * Top user per day/week/month
 * Lowest user per day/week/month
 * Save to database.
 */


/*
 * HouseDb will be able to create its own OverviewDataDb for per day.
 * The tasks related to the top/lowest per day/week/month will be handle by other code.
 * Perhaps, just perhaps the house should not be concerned about creating OverViewDataDb at all
 * Seperation of concerns, houseDb just handle storing its data, while something else handle data calculations.
 * Need to handle out of order data, e.g. 1st May, then 2nd May and then 1st May again.
 */