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

## Authentication
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