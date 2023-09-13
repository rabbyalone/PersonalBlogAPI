
# PersonalBlog API
The PersonalBlog API serves as the backend for the stylish React blog template available at [myblog](https://github.com/rabbyalone/myblog). This template was originally cloned from [tailwind-nextjs-starter-blog](https://github.com/timlrx/tailwind-nextjs-starter-blog) by timlrx. While the original template utilized MDX parsing, this version has been adapted to function as an API, providing support for various features necessary for a personal blog.

## Features 

This API provides several common endpoints and functionalities that support the myblog template:

- **Token-Based Authorization:** Secure your API with token-based authorization using user secrets.
- **View Summary Posts:** Retrieve summaries of blog posts.
- **View Posts by Tag:** Filter blog posts by tags.
- **Get All Tags:** Fetch a list of all available tags.
- **Create Post:** Add new blog posts.
- **Update and Delete Post:** Edit or remove existing blog posts

## Getting Started

#### Step 1: Create a MongoDB Atlas Database

To get started, you'll need to set up a MongoDB Atlas database. Follow these steps:

- Visit [MongoDB Atlas](https://www.mongodb.com/atlas/database) and sign up for an account if you don't already have one.

- Create a new database and collection within MongoDB Atlas. You can take advantage of the free forever tier, which includes 512MB of storage.

- Generate a connection string for your MongoDB Atlas database. You'll need this connection string to establish a connection between your API and the database.

#### Step 2: Configure Secrets

In the project, you'll need to configure various secrets, including user secrets used for token-based authorization. Here's what you need to do:

- Ensure you've set up the JwtSetting:UserSecret in your configuration. This secret is used to generate authorization tokens.

```
 "MongoSettings": {
    "MongoConnection": "See user Secret",
    "Password": "See user Secret",
    "DatabaseName": "see user secret",
    "PostCollectionName": "see user secret"
  },
  "JwtSettings": {
    "SecretKey": "See user Secret",
    "Issuer": "See user Secret",
    "Audience": "see user secret",
    "UserSecret": "see user secret"
  }
```
#### Deploy
I have used Azure web apps free forever version for deployment of this application.

By following these steps, you'll have your PersonalBlog API ready to serve your blog template. With token-based authorization and the ability to manage blog posts and tags, you can create and maintain your own personal blog with ease. Enjoy the benefits of deploying your blog while utilizing free resources.

