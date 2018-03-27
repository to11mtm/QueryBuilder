QueryBuilder - Eliminate all your SQL Typos!

QueryBuilder is meant to be a simple example of how easy it is to write a basic SQL Parser that is still fluid to work with in C#.

This isn't meant to be a silver bullet. It was a fun, 2-3 hour project. It can be handy for simple CRUD applications, as it handles both Retrieving and Updating Data with both conditional SET and WHERE clause building.

There are some significant rough edges, however:
 - The two most frequent Insert Scenarios I see are to Either (1) Return the Inserted ID of the object, or (2) return the object as updated from DB (I.e. Defaults, Triggers, etc.)
   - Scenario 1 cannot be done implicitly in Oracle without Attributes
     - We get around this by providing an overload that requires an ID Selector.
	 - The SQL-Server Specific Implementation still uses SCOPE_IDENTITY(); The amount of boilerplate for OUTPUT just isn't worth it for this use case.
   - Scenario 2 Is a different method, and users should be aware of performance implications:
     - SQL Server Implementation uses a Select-After-Insert.
	    - SQL Server Doesn't fire triggers before OUTPUT generates.
	      - Blame Microsoft
 
  - Some of the Language Design (Ab)use is questionable on my part. Namely the `and`/`or` Properties.
    - Comment them out or use Conditionals in preprocessor.

 - There's almost no caching of reflection data here
  - Can be added later
  - I'm not sure how cacheable what we are using is, given how simple the use-cases are.

 - This is a design that probably could use some cleanup right out of the gate.
   - I am least proud of how we are assembling the SQL. Specifically parameters.
     - Especially the Parameter Character.
     - At some point this will be refactored so that parameters are assembled by the Db Layer (i.e. QueryBuilder.Dapper) instead.

 Things that would be cool to have, but are not a priority. Code for this is not in a working state:

  - Joins: It's just super painful to get good syntax.
   - Dapper Join Syntax is tricky to build on the fly, especially when IDs are not called 'ID'
   - The In-Progress implementation is based on Multiple-Queries on a single Roundtrip. You can only join two tables together.
     - Implementing more joins probably gets tricky. 

 Things that aren't here that I'll likely never care about:
  - Oracle Specific Behavior for Huge (1000+) items in a list for a Where clause. 
    - It can be done, but it's not a good practice most of the time anyway; sending that many variables over the wire will hurt, to say nothing of what your DBA will say when they see the query plan.
