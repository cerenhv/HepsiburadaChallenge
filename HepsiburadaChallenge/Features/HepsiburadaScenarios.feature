Feature: HepsiburadaScenarios

Scenario 1: Search products in Hepsiburada and add them to the basket
Scenario 2: Update user informations

Scenario: Scenario 1
Given I open the Chrome browser and go to Hepsiburada website.
And I login with 'testhb.ceren@gmail.com' and 'testCeren123'
When I search 'laptop' in product catalog
And I add 1 and 3 products to the basket
And I go to the basket and view added products
And I increase amounts of products and check the amounts
Then I go back to home page
And I logout from Hepsiburada


Scenario: Scenario 2
Given I open the Chrome browser and go to Hepsiburada website.
And I login with 'testhb.ceren@gmail.com' and 'testCeren123'
And I go to user informations
When I change birth date
Then I update the informations and check the date
And I go back to home page
And I logout from Hepsiburada