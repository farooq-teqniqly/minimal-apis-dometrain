Feature: BookValidation

Tests for ensuring errors are returned when an invalid Book is given to a post request.

@tag1
Scenario: The book request contains an invalid isbn
	Given the book request body contains an invalid isbn
	When the post request is made
	Then a 400 bad request status is returned
