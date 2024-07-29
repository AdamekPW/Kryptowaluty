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

![image](https://github.com/user-attachments/assets/03c554de-f7cd-4830-a9b1-33262236c4c5)
![image](https://github.com/user-attachments/assets/e737b4bb-fb4e-4d78-b17e-ce8f7193f9e4)
![image](https://github.com/user-attachments/assets/b73a78a7-bbdf-4b1c-9686-f3dec0e3e42c)
![image](https://github.com/user-attachments/assets/9c722d40-e9bf-4df8-9183-db71f1e4152a)
![image](https://github.com/user-attachments/assets/b346cebe-b0ac-4b33-a864-56971019c96e)
![DiagramEncji](https://github.com/user-attachments/assets/b7b58482-e186-4312-a929-70a8ada988c1)
