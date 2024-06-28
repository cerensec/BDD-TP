Feature: Scrutin

Scrutin scenarios

Scenario: A candidate wins with over 50% of the votes
	Given the voting is closed
	And the candidates and their votes are
	| name      | votes  |
	| Carlos    | 60     |
	| Bob       | 40     |
	When we count the votes
	Then the winner should be Carlos
	And the winner count should be
	| name      | votes  | votePercentage |
	| Carlos    | 60     | 60             |
	| Bob       | 40     | 40             |

Scenario: No candidate wins with over 50% of the votes
	Given the voting is closed
	And the candidates and their votes are
	| name      | votes  |
	| Carlos    | 45     |
	| Bob       | 30     |
	| Eduardo   | 25     |
	When we count the votes
	Then there should be a second round
	And the remaining candidates should be
	| name      | votes  | votePercentage |
	| Carlos    | 45     | 45             |
	| Bob       | 30     | 30             |


Scenario: A candidate wins with over the 50% of the votes on the second round
	Given the voting is closed
	And the first round results are
	| name   | votes | votePercentage |
	| Bob    | 45    | 45             |
	| Carlos | 55    | 55             |
	When the second round results are calculated
	Then the winner should be Carlos
	And the vote counts should be
	| name   | votes | votePercentage |
	| Carlos | 55    | 55             |
	| Bob    | 45    | 45             |

Scenario: Tie in the second round
	Given the voting is closed
	And the first round results are
	| name   | votes | votePercentage |
	| Bob    | 50    | 50             |
	| Carlos | 50    | 50             |
	When the second round results are calculated
	Then there should be a tie
	And the vote counts should be
	| name   | votes | votePercentage |
	| Carlos | 50    | 50             |
	| Bob    | 50    | 50             |

 Scenario: Equality between 2nd and 3rd candidate in the first round
    Given the voting is closed
    And the candidates and their votes are
      | name    | votes |
      | Alice   | 40    |
      | Bob     | 30    |
      | Carlos  | 30    |
    When we count the votes
    Then there should be a second round
    And the remaining candidates should be
      | name    | votes  | votePercentage |
      | Alice   | 40     | 40             |
      | Bob     | 30     | 30             |
      | Carlos  | 30     | 30             |

Scenario: Carlos wins with over 50% of the votes including blank votes
  Given the voting is closed
  And the candidates and their votes are
    | name   | votes |
    | Carlos | 60    |
    | Bob    | 40    |
  And there are 20 blank votes
  When we count the votes
  Then the winner should be Carlos
  And the winner count should be
    | name   | votes | votePercentage |
    | Carlos | 60    | 50.0           |
    | Bob    | 40    | 33.3           |
    | Blank  | 20    | 16.7           |
