Feature: Book

The Book feature consists of all API endpoints that handle Books.

Scenario: Adding a book
	Given a book with the given isbn does not exist
	When a valid post request is made
	Then a 201 created status is returned
	Then the location header is set
	Then the response body contains the added book
	Then the book in the response has an isbn
	Then the book in the response has an author
	Then the book in the response has a title
	Then the book in the response has a page count
	Then the book in the response has a release date

Scenario: Retrieving a book
	Given a book with the given isbn has been added
	When a get request is made
	Then a 200 ok status is returned
	Then the response body contains the retrieved book
	Then the retrieved book has an isbn 
	Then the retrieved book has an author 
	Then the retrieved book has an title 
	Then the retrieved book has an page count 
	Then the retrieved book has an release date 