# It Takes A Village

## Purpose
The idea behind our project, "It takes a village," is to help people collaborate on everyday tasks. It may particularly focus on families with small children but will be applicable to a much broader audience. We aim to steer away from individualism and instead facilitate activities such as communal cooking, childcare, playdates, item-sharing pools including tools, joint ownership of seldom-used items, and much more. Our goal is that this approach could also contribute to environmental efforts by cooking together, sharing tools and items, and reducing overall consumption.

## Usage
#### Login
Create an account if you don't have one. When you have an account, just press the login button and enter your email and password to log in,

#### Create group
If you're not member of a group you either have to create a new group or ask a friend to invite you to a group of theirs. Creating of a group just needs you to put in a name and save. You can't create two groups with the same or very similar names to a group you are already a member of.

#### Create Dinner Invitation
Like mentioned above, you have to be a member of a group to be able to create a new dinner invitation. Dinner invitations is not for fancy dinners, but for every day meals that you share with your friends.  
To create a invitation you have to select one of your groups, the invitation will be sent to all members of the group. You also have to select a date and time. It is not possible to create a invitation back in time, it has to be today or in the future. Course, location and additional information are optional fields.  
When you press Save, a notification will be sent to all members of the group.

#### Create Play Date
Play date invitations is very similar to dinner invitations, with some alterations in the optional fields. 

#### Tool Pool
This is a future feature. The idea is that you can borrow tools and other things from the members of a group.

## Structure
The application is a .NET Core Web application built with Razor Pages. The unit tests uses Xunit. The Razor page app uses Entity Framework and .NET's built in Identity for logging in and handling user accounts. It follows the repository pattern where the repository is generic so that the services can create dependencies of a repository of a chosen object. Our business logic is placed in services, to keep as much logic as possible out of the PageModels. In the future, the plan is to split the project into a structure that follows more of the clean architecture principles.