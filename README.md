General info:
This is a project for a simple cryptocurrency exchange, through which you can 
simulate transactions without actually incurring costs (or profits).
Each newly created user gets 1000 USDT to start, which they can exchange for any of 10 cryptocurrencies of my choice. 
This is my first project is entirely implemented in ASP .NET core MVC, so I am aware of possible errors.
Microsoft SQL Server has been used as the database and I have used Entity Framework Core 
to communicate with it along with LINQ to build the queries. 
For viewing and manual editing of the data I use Microsoft SQL Server Management Studio.
The relational database schema was entirely designed by me as an entity diagram and ported to MSS (Code-First).
I used Code-First because, the schema was built as the project was being developed, 
so this approach made the work much easier.


The data updates every minute - the current exchange rate of each currency is taken and the 24h low, 
24h high and change relationships are calculated. When the server is started, 
an additional function is run to retrieve periodic data, i.e. the system looks at the last known record in history 
and if that record occurred more than one day ago, 
the system calculates the difference in days between the last date and today's date, 
retrieves the missing data and calculates the necessary parameters (in this case there is some approximation, 
as CoinGecko allows access to 24 measurements from a single day).

