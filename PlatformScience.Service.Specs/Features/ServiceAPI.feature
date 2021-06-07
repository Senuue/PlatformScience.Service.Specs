Feature: API
	Tests for the "hoover" API

@RestartService
Scenario: The default scenario works as intended
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 1      | 0      |
| 2      | 2      |
| 2      | 3      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should get the following ending coords <endX> and <endY>
And I should have <patchesCount> patches
Examples:
| dimensionX | dimensionY | startX | startY | instructions | endX | endY | patchesCount |
| 5          | 5          | 1      | 2      | NNESEESWNWW  | 1    | 3    | 1            |

@RestartService
Scenario Outline: The cleaning-sessions endpoint can clean all patches
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
| 0      | 1      |
| 0      | 2      |
| 0      | 3      |
| 0      | 4      |
| 1      | 0      |
| 1      | 1      |
| 1      | 2      |
| 1      | 3      |
| 1      | 4      |
| 2      | 0      |
| 2      | 1      |
| 2      | 2      |
| 2      | 3      |
| 2      | 4      |
| 3      | 0      |
| 3      | 1      |
| 3      | 2      |
| 3      | 3      |
| 3      | 4      |
| 4      | 0      |
| 4      | 1      |
| 4      | 2      |
| 4      | 3      |
| 4      | 4      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should get the following ending coords <endX> and <endY>
And I should have <patchesCount> patches
Examples:
| dimensionX | dimensionY | startX | startY | instructions             | endX | endY | patchesCount |
| 5          | 5          | 0      | 0      | NNNNESSSSENNNNESSSSENNNN | 4    | 4    | 25           |

@NoServiceRestart
Scenario Outline: Hoover can not go off grid
Given I set the room size to 5 by 5
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should get the following ending coords <endX> and <endY>
Examples:
| startX | startY | instructions | endX | endY |
| 0      | 0      | S            | 0    | 0    |
| 0      | 0      | W            | 0    | 0    |
| 0      | 1      | W            | 0    | 1    |
| 0      | 2      | W            | 0    | 2    |
| 0      | 3      | W            | 0    | 3    |
| 0      | 4      | W            | 0    | 4    |
| 0      | 4      | N            | 0    | 4    |
| 1      | 4      | N            | 1    | 4    |
| 2      | 4      | N            | 2    | 4    |
| 3      | 4      | N            | 3    | 4    |
| 4      | 4      | N            | 4    | 4    |
| 4      | 4      | E            | 4    | 4    |
| 4      | 3      | E            | 4    | 3    |
| 4      | 2      | E            | 4    | 2    |
| 4      | 1      | E            | 4    | 1    |
| 4      | 0      | E            | 4    | 0    |
| 4      | 0      | S            | 4    | 0    |
| 3      | 0      | S            | 3    | 0    |
| 2      | 0      | S            | 2    | 0    |
| 1      | 0      | S            | 1    | 0    |

@NoServiceRestart
Scenario Outline: Service handles invalid inputs for roomSize
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should receive a BadRequest result
Examples:
| dimensionX | dimensionY | startX | startY | instructions |
| 0          | 0          | 0      | 0      | S            |
| 5          | 0          | 0      | 0      | S            |
| 0          | 5          | 0      | 0      | S            |
| -1         | 5          | 0      | 0      | S            |
| 5          | -1         | 0      | 0      | S            |

@NoServiceRestart
Scenario Outline: Service handles invalid inputs for coordinates
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should receive a BadRequest result
Examples:
| dimensionX | dimensionY | startX | startY | instructions |
| 5          | 5          | 9      | 0      | S            |
| 5          | 5          | 0      | 9      | S            |
| 5          | 5          | -1     | 0      | S            |
| 5          | 5          | 0      | -1     | S            |

@NoServiceRestart
Scenario Outline: Service handles invalid inputs for instructions
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should receive a BadRequest result
Examples:
| dimensionX | dimensionY | startX | startY | instructions |
| 5          | 5          | 0      | 0      | R            |
| 5          | 5          | 0      | 0      | @            |

@NoServiceRestart
Scenario: Service handles invalid inputs for patches, negative X
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| -1     | 0      |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should receive a BadRequest result
Examples:
| dimensionX | dimensionY | startX | startY | instructions |
| 5          | 5          | 0      | 0      | S            |

@NoServiceRestart
Scenario: Service handles invalid inputs for patches, negative Y
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to <startX> and <startY>
And I set dirt patches at
| patchX | patchY |
| 0      | -1     |
And I set the following instructions <instructions>
When I call the cleaning-sessions endpoint
Then I should receive a BadRequest result
Examples:
| dimensionX | dimensionY | startX | startY | instructions |
| 5          | 5          | 0      | 0      | S            |

@NoServiceRestart
Scenario Outline: I can create and traverse grids of varying sizes
Given I set the room size to <dimensionX> by <dimensionY>
And the starting coordinates to 0 and 0
And I set dirt patches at
| patchX | patchY |
| 0      | 0      |
And I create instructions to traverse the entire grid from <dimensionX> and <dimensionY>
When I call the cleaning-sessions endpoint
Then I should get the expected end coordinates from traversing a <dimensionX> by <dimensionY> grid
Examples: 
| dimensionX | dimensionY |
| 9          | 7          |
| 25         | 32         |
| 1          | 5          |
| 5          | 1          |
| 250        | 250        |
| 4          | 4          |
| 5          | 4          |
| 3000       | 23         |