# Introduction to Blazor Web App 8
Blazor Web App 8 is the latest iteration of the innovative web framework by Microsoft, designed to enable developers to build interactive and modern web applications using C# and .NET instead of JavaScript. This framework allows the development of client-side web applications that run directly in the browser using WebAssembly, as well as server-side applications that utilize SignalR for UI updates.

## What's New in Blazor Web App 8
Blazor Web App 8 introduces several enhancements and new features aimed at improving performance, development experience, and integration capabilities. These updates include:

* Performance Improvements: Enhanced rendering speed and reduced application size for faster load times.
* New Component Features: Additional component capabilities for building richer user interfaces.
* Improved Development Tools: Enhanced tooling support in Visual Studio and Visual Studio Code for a more efficient development process.
* Advanced Integration Options: Better integration with modern JavaScript libraries and APIs, allowing for more flexible and powerful web applications.

## Creating a Project
To create a Blazor Web App in Visual Studio 2022, first ensure you have the latest version of Visual Studio with the ASP.NET and web development workload installed. Then, create a new project and select the "Blazor Web App" template, which supports both interactive server-side rendering (SSR) and client-side rendering (CSR). This template is recommended for learning about Blazor's server-side and client-side features. For more detailed instructions on setting up your project, including choosing render modes and configuring for client or server-side rendering, you might want to consult the official Microsoft documentation

## Rendering Modes
The Blazor Web App template in Visual Studio 2022 notably includes three rendering modes for applications: Server, WebAssembly (Client), and Automatic. The Server mode executes application logic on the server, utilizing SignalR for dynamic UI updates. The WebAssembly mode allows the application to run directly in the browser, leveraging .NET on WebAssembly for client-side execution. The innovative Automatic mode intelligently combines both, starting with server-side rendering for a quick initial load and transitioning to WebAssembly for rich interactivity, optimizing both performance and the user experience. For more detailed insights, you're encouraged to refer to the official Microsoft documentation on Blazor project templates and rendering modes.

## Localization in Blazor Applications
Localization in Blazor applications refers to the process of adapting your application to support multiple languages and cultures. This is crucial for developing global applications that cater to a diverse user base.

### Microsoft.Extensions.Localization Middleware
* The Microsoft.Extensions.Localization middleware is a key component for enabling localization in both server and client-side Blazor applications. It provides services and interfaces for retrieving localized strings from resource files, allowing developers to create multilingual applications efficiently.

### Server-Side Localization
On the server side, localization involves configuring the middleware in the Startup.cs or Program.cs file and using it to serve localized content based on the user's preferences or settings.

### Client-Side Localization
For client-side Blazor applications, localization can be achieved by integrating the localization library and ensuring that the application loads and applies the appropriate resources based on the user's language settings.

### Organizing Localization Resources
Organizing localization files in a specific folder, separate from the Blazor components and pages, offers several advantages:

* Modularity: Keeps localization resources centralized, making them easier to manage and update.
* Scalability: Facilitates the addition of new languages and cultures without cluttering the application structure.
* Separation of Concerns: Maintains a clear separation between UI components and localization resources, adhering to good software development practices.

* This organization strategy enhances the maintainability and scalability of Blazor applications, making it simpler to support multiple languages and cultures as the application grows.

By adopting these practices and leveraging Blazor Web App 8's features, developers can create robust, modern web applications that are accessible to a global audience.

## how to use our template
To implement localization in your code using our repository, fork or download our specific functional branch named feature/localization. Follow the provided to-do list, as each step of the code implementation is detailed there. This structured approach ensures a thorough understanding and integration of localization features into your Blazor application.

### todo-list
#### server side
1. create on server side project a folder named Locales
2. create on the server side project a resource file named AppResource.resx and generate it code
3. add all your resources keys
4. add one resources file for each culture 
5. Reference Microsoft.Extensions.Localizations package

#### client side
1. create on client side project a folder named Locales
2. create on the client side project for each client component and page blazor named.resx and generate it coe
3. add all your resources keys
4. add one resource file for each component and page and each culture
5. Reference Microsoft.Extensions.Localizations package

#### following the code todo-list
1. display todolist
2. view or change code include to adapt to your project 

## Implements Authentication
This module focuses on implementing server-side authentication using *Microsoft.AspNetCore.Identity.EntityFrameworkCore*. 
We'll explore creating a login form that utilizes email and password for user authentication. 
A significant portion will be dedicated to implementing two-factor authentication, enhancing the security of our application.

On the client-side, we'll delve into Microsoft.AspNetCore.Components.WebAssembly.Authentication, discussing how to implement AuthenticationStateProvider and a redirection component for handling unauthenticated users or expired sessions.

Additionally, we'll cover the integration of Single Sign-On (SSO) with social media accounts like Facebook and Google, providing a streamlined login experience.

Finally, an important aspect for enterprises, the integration with Microsoft Identity Platform for authentication using Azure AD, will be discussed, offering a robust solution for managing user identities and permissions.

This README outlines the key components and steps involved in securing your application, providing a solid foundation for both server and client-side authentication strategies.

### implement User Flow with login and Password
[Read this readme](./README.Authentication.md)

### Managing Personal Data in ASP.NET Core Identity
ASP.NET Core Identity provides a robust framework for managing user authentication and authorization in web applications. A key aspect of this framework is its built-in support for privacy and data protection, particularly through the management of personal data and integration with external login providers.

#### Personal Data Management with PersonalDataAttribute
- Core Functionality: ASP.NET Core Identity allows developers to mark specific properties within the user model (e.g., IdentityUser) as personal data using the PersonalDataAttribute. This attribute signals to the framework that the data is sensitive and should be handled with care in operations such as data export or deletion.
- Data Export: When a user requests to download their personal data, the system dynamically scans for properties decorated with PersonalDataAttribute and includes this data in the export, typically in a JSON format. This ensures comprehensive coverage of all personal data deemed sensitive by the developer.
- Customization and Flexibility: Developers have the flexibility to define what constitutes personal data within their application by decorating relevant properties in the user entity. This allows for the inclusion of additional information beyond standard fields, ensuring a tailored approach to data privacy.

#### Integration with External Logins
- Seamless Authentication: ASP.NET Core Identity supports authentication through external providers (e.g., Google, Facebook, Twitter), allowing users to sign in with their accounts from these services. This feature simplifies the login process and enhances user experience.
- Handling External Login Data: Information related to a user's external logins, including the provider name and user identifier within the external service, is considered part of the user's personal data. This information is included in the data export to provide a complete picture of the user's data footprint within the application.
- Transparency and Control: Including external login details in the personal data export underscores the application's commitment to transparency and user control over their data. It enables users to understand how their data is interconnected with external services and make informed decisions regarding their privacy.

#### Conclusion
ASP.NET Core Identity's approach to personal data management, characterized by the PersonalDataAttribute and the integration of external logins, reflects a comprehensive strategy for privacy and data protection. By providing mechanisms for data export and deletion, along with clear indications of how and where personal data is used, ASP.NET Core Identity helps developers build applications that respect user privacy and comply with data protection regulations like GDPR. This framework empowers users with control over their personal information, fostering trust and security in the digital environment.

## Authorization
Authorization in ASP.NET Core is a crucial aspect of building secure web applications. It involves defining and enforcing access policies that determine which users are allowed to perform specific actions or access certain resources within the application. ASP.NET Core provides a flexible and extensible authorization system that supports a wide range of scenarios, from simple role-based access control to complex, fine-grained authorization rules.

### Role-Based Authorization
Role-based authorization is a common approach to managing access control in web applications. It involves associating users with specific roles (e.g., "admin," "manager," "user") and defining access policies based on these roles. ASP.NET Core provides built-in support for role-based authorization, allowing developers to easily define and enforce role-based access policies.

#### Implementing Role-Based Authorization
To implement role-based authorization in an ASP.NET Core application, follow these general steps:

1. Define Roles: Define the roles that users can be assigned to within the application. This can be done using the RoleManager service provided by ASP.NET Core Identity, or by creating custom role management logic.
2. Define on server side for each page the role needed to access the page : @attribute [Authorize(Roles = "Admin")]
3. You can also define a role for a specific zone of the page example: &lt;AuthorizeView Roles="Admin"&gt;

### Policy-Based Authorization
Policy-based authorization is a more flexible and granular approach to access control, allowing developers to define custom authorization policies based on a wide range of criteria, including user attributes, resource properties, and external factors. ASP.NET Core provides a powerful policy-based authorization system that enables developers to define and enforce complex access rules.

#### Implementing Policy-Based Authorization
To implement policy-based authorization in an ASP.NET Core application, follow these general steps:

1. Define Policies: Define custom authorization policies using the AuthorizationOptions service in the ConfigureServices method of the application's Startup class. Policies can be based on a wide range of criteria, including user attributes, resource properties, and external factors.
2. Define on server side for each page the policy needed to access the page : @attribute [Authorize(Policy = "PolicyName")]
 
### Deploy on Docker
Docker is a powerful platform for building, shipping, and running applications in containers. It provides a consistent environment for applications to run across different environments, making it an ideal choice for deploying ASP.NET Core applications.

#### Single-Container vs Multi-Container Deployment
Single-container deployment involves running an application in a single container, while multi-container deployment involves running an application in multiple interconnected containers. The choice between single-container and multi-container deployment depends on the complexity and scalability requirements of the application.
The deployment on single-container is the most common and the easiest to implement. The deployment on multi-container is more complex and is used for more complex applications.

- By single container, I mean that the application is deployed in a single container, and the database is deployed in the same on in another container. The two containers are interconnected.
- By multi-container, I mean that the application is deployed on 2 or more containers, and the database is deployed in another container. The front containers and the database container are interconnected.

#### Deploying an ASP.NET Core Application in a single Docker Container
To deploy an ASP.NET Core application in a single Docker container, follow these general steps:

1. Migrate your environment to Docker: Ensure that your development environment is set up to work with Docker. This may involve installing Docker Desktop, setting up Docker Compose, and configuring your application to work with Docker.
2. Migrate your dbcontext to work with Docker: Ensure that your dbcontext is configured to work with Docker. This may involve using a DBMS compatible with Docker and setting up the connection string to work with Docker.
3. Create a Dockerfile: Create a Dockerfile in the root of the ASP.NET Core application. The Dockerfile contains instructions for building the Docker image, including the base image, application files, and runtime configuration.	
4. Create a docker-compose.yml file: create a docker-compose.yml file in the root of the ASP.NET Solution

on a single container and if you are using a database on the same container, you can use sqllite. If you are using a database on another container, you can use the following databases:
- SQL Server
- MySQL
- PostgreSQL

##### use env file for secrets.json migration
- to migrate your secret file on your container you can use a .env file but you should exclude it from your git repository. You can also use the secret manager to store your secret.
- Because .env file is ascii simple text file you can not include json, so you will need translate your tree in ascii format, you can separate level by __

### Deploying an ASP.NET Core Application in a multi Docker Container
To deploy an ASP.NET Core application in a multi Docker container, follow these general steps:

#### Architecture for a multi-container deployment
##### Services
- Frontend: The ASP.NET Core application running in a container.
- Database: The database running in a separate container.
- Cache: The cache running in a separate container.
- Data Directory: To store key generate for dataprotection
- Nginx: The reverse proxy running in a separate container.

##### Database
To deploy an ASP.NET Core application in a multi-container deployment, you will need to set up a database in a separate container. You can use the following databases:
- SQL Server
- MySQL
- PostgreSQL

In our sample we use SQL Server, but it can be easy to change it to another database.

##### Session State
To deploy an ASP.NET Core application in a multi-container deployment, you will need to set up a session state in a separate container. You can use the following session state:
- Redis
- SQL Server
- CosmosDB

In our sample we use Redis, but it can be easy to change it to another session state.

###### Data Directory
To deploy an ASP.NET Core application in a multi-container deployment, you will need to share your key generated by the system between the front and the session state. You can use a volume to share the key between the front. You can use several mechanism but the easier is to use a volume.

In our sample we use a volume, but it can be easy to change it to another mechanism.

##### Reverse Proxy
To deploy an ASP.NET Core application in a multi-container deployment, you will need to set up a reverse proxy in a separate container. You can use the following reverse proxy:
- Nginx
- Apache

In our sample we use Nginx, but it can be easy to change it to another reverse proxy.
