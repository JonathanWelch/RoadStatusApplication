Feature: RoadStatus
	An application for displaying the status of major roads.

Background: 
Given we have the following valid roads
| RoadId           | DisplayName      | Status | StatusDescription     |
| a2               | A2               | Good   | No Exceptional Delays |
| blackwall tunnel | Blackwall Tunnel | Bad    | Long Delays           | 

Scenario: Retrieve the status of a valid road
	Given a valid road id of a2 is specified
	When the client is run
	Then the road A2 should be displayed
		And the road status should be Good
		And the road status description is No Exceptional Delays

Scenario: Retrieve the status of a valid road with a bad status
	Given a valid road id of blackwall tunnel is specified
	When the client is run
	Then the road Blackwall Tunnel should be displayed
		And the road status should be Bad
		And the road status description is Long Delays

Scenario: Retrieve the status of an invalid road
	Given an invalid road id of a233 is specified
	When the client is run
	Then the application should return an informative error 
		And the application should exit with a non-zero System Error code