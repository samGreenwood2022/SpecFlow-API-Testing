Feature: Posts API

Scenario: Retrieve an existing post
  When I send a GET request to "/posts/1"
  Then the response status should be 200
  And the response body should contain "userId" with value "1"

Scenario: Create a new post
  When I send a POST request to "/posts" with title "My test post"
  Then the response status should be 201
  And the response body should contain "title" with value "My test post"

Scenario: Request a post that does not exist
  When I send a GET request to "/posts/99999"
  Then the response status should be 404

Scenario: Update an existing post
	When I send a PUT request to "/posts/1" with title "Updated test post"
	Then the response status should be 200
	And the response body should contain "title" with value "Updated test post"	

Scenario: Delete an existing post
	When I send a DELETE request to "/posts/1"
	Then the response status should be 200
	And the response body should be blank