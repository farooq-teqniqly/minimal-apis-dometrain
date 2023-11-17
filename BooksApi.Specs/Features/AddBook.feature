Feature: AddBook

A short summary of the feature

@BooksResource
Scenario: Adding a book
	Given when the book with the same ISBN does not exist
	When a valid POST request is made
	Then a "201 Created" status is returned
	Then the location header will be set to the book's location
	Then the book is in the response body
